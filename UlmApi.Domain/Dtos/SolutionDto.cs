using UlmApi.Domain.Entities;

namespace UlmApi.Domain.Dtos
{
    public class SolutionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string ProductName { get; set; }

        public SolutionDto(Solution solution)
        {
            Id = solution.Id;
            Name = solution.Name;
            OwnerId = solution.OwnerId;
            OwnerName = solution.OwnerName;
            ProductName = solution.Product.Name;
        }
    }
}