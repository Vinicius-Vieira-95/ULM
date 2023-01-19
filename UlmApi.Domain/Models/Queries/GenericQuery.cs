namespace UlmApi.Domain.Models.Queries
{
    public abstract class GenericQuery
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; } 

        public GenericQuery()
        {
            Limit = 10;
            Page = 0;
        }
    }
}