namespace UlmApi.Application.Extensions
{
    public static class Policies
    {
        public const string ADMIN = "ADMIN";
        public const string OWNER = "OWNER";
        public const string ADMIN_OR_OWNER = "ADMIN_OR_OWNER";
        public const string REQUESTER = "REQUESTER";
    }
}