using System;

namespace UlmApi.Infra.CrossCutting.RabbitMQ.Payloads
{
    public class TPayload
    {
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public DateTime Date { get; private set; }
        public string Operation { get; set; }
        public string Entity { get; set; } 
        public string Message { get; set; }
        public object Body { get; set; }

        public TPayload(string userId, string userRole, string operation, string entity, string message, object body)
        {
            UserId = userId;
            UserRole = userRole;
            Operation = operation;
            Message = message;
            Body = body;
            Entity = entity;
            Date = DateTime.Now;
        }

        public TPayload() 
        {
            Date = DateTime.Now;
        }
    }
}