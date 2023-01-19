namespace UlmApi.Domain.Models.Queries
{
    public class GetUsersQuery : GenericQuery
    {
        public bool Pagineted { get; set; }

        public GetUsersQuery()
        {
            Pagineted = true;
            Limit = 10;
            Page = 0;
        }

        public GetUsersQuery(bool pagineted)
        {
            Pagineted = pagineted;
            Limit = 10;
            Page = 0;
        }
    }
}