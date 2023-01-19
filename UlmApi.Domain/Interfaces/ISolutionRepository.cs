using System.Collections.Generic;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface ISolutionRepository : IBaseRepository<Solution, int>
    {
        Task<List<SolutionDto>> GetSolutions();
        Task<List<SolutionDto>> GetSolutions(int limit);
        Task<SolutionsListDto> GetSolutionsWithPagination(GetSolutionsQuery queryParams);
        Task<SolutionDto> GetSolutionByOwnerId(string ownerId);
        Task<SolutionDto> GetSolutionByName(string solutionName);
    }
}
