using NLog;
using System;

namespace UlmApi.Infra.CrossCutting.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager() {}

        public void LogError(string msg)
        {
            logger.Error(msg);
        }

        public void LogError(Exception exception)
        {
            logger.Error(exception);
        }

        public void LogError(string msg, object obj)
        {
            logger.Error(msg, obj);
        }

        public void LogError(string msg, object obj, Exception exception)
        {
            logger.Error(msg, obj, exception);
        }

        public void LogInfo(string msg)
        { 
            logger.Info(msg);
        }

        public void LogInfo(string msg, object obj)
        {
            logger.Info(msg, obj);
        }

    }
}
