using System.Collections.Generic;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface IApplicationRepository : IBaseRepository<Application, int>
    {
        Task<ApplicationListDto> GetApplicationWithPagination(GetApplicationPaginationQuery queryParams);
        Task<List<ApplicationDto>> GetApplications();
        Task<List<ApplicationDto>> GetApplications(int limit);
        Task<ApplicationDto> GetApplicationByName(string applicationName);
    }
}
