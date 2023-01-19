using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UlmApi.Domain.Attributes;
using UlmApi.Domain.Models.Queries;
using UlmApi.Infra.CrossCutting.Logger;
using UlmApi.Infra.CrossCutting.RabbitMQ;
using UlmApi.Infra.CrossCutting.RabbitMQ.Payloads;
using UlmApi.Service.Services;

namespace UlmApi.Service.Interceptors
{
    public class InterceptDecorator<T> : DispatchProxy
    {
        public IEventBus _eventBus { get; set; }
        private ILoggerManager _logger;
        private AuthenticatedUserService _authenticatedUser;
        private T _decorated;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            dynamic process;
            process = targetMethod.Invoke(_decorated, args);
            var resultAsTask = process as Task;

            try
            {
                if (targetMethod.CustomAttributes.Any())
                {
                    LogAfter(targetMethod, args, resultAsTask == null ? process : process.Result);
                }
                return process;
            }
            catch (Exception ex)
            {
                LogException(ex, targetMethod);
                return process;
            }
        }

        public static T Create(T decorated, IServiceProvider serviceProvider)
        {
            object proxy = Create<T, InterceptDecorator<T>>();
            ((InterceptDecorator<T>)proxy).SetParameters(decorated, serviceProvider);

            return (T)proxy;
        }

        private void SetParameters(T decorated, IServiceProvider serviceProvider)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(nameof(decorated));
            }
            _decorated = decorated;
            _eventBus = serviceProvider.GetService<IEventBus>();
            _logger = serviceProvider.GetService<ILoggerManager>();
            _authenticatedUser = serviceProvider.GetService<AuthenticatedUserService>();
        }

        private void LogException(Exception exception, MethodInfo methodInfo)
        {
            _logger.LogError($"Class {_decorated.GetType().FullName}, Method {methodInfo?.Name} threw exception:\n{exception}");
        }

        private void LogAfter(MethodInfo methodInfo, object[] args, object result)
        {
            var attributes = GetLogAttributes(methodInfo);
            var operationString = Enum.GetName(typeof(Operation), attributes.Operation);
            var payload = new TPayload 
            {
                UserId = _authenticatedUser.Id,
                UserRole = _authenticatedUser.Role,
                Entity = attributes.Entity,
                Message = attributes.Message,
                Operation = operationString
            };

            if (attributes.Operation == Operation.QUERY)
            {
                var query = args.Where(q => q.GetType().IsSubclassOf(typeof(GenericQuery))).FirstOrDefault();
                payload.Body = query;
                _eventBus.Publish(payload, "queue_logs");
            }

            else
            {
                payload.Body = result;
                _eventBus.Publish(payload, "queue_logs");
            }

            if (attributes.Operation == Operation.CREATE && attributes.Entity == "RequestLicense")
                _eventBus.Publish(payload, "queue_requests_created");

            _logger.LogInfo(attributes.Message);
        }

        private LogAttribute GetLogAttributes(MethodInfo methodInfo)
        {
            var operation = methodInfo.CustomAttributes.FirstOrDefault().ConstructorArguments[0].Value;
            var entity = methodInfo.CustomAttributes.FirstOrDefault().ConstructorArguments[1].Value as string;
            var message = methodInfo.CustomAttributes.FirstOrDefault().ConstructorArguments[2].Value as string;

            return new LogAttribute((Operation) operation, entity, message);
        }

    }
}
