namespace UlmApi.Domain.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Interface)]
    public class LogAttribute : System.Attribute
    {
        public Operation Operation { get; private set; }
        public string Entity { get; private set; }
        public string Message { get; set; }

        public LogAttribute(Operation operation, string entity, string message)
        {
            Operation = operation;
            Entity = entity;
            Message = message;        
        }
    }
}