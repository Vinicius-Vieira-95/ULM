using UlmApi.Domain.Entities;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Models.Queries;
using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Interfaces
{
    public interface ILicenseRepository : IBaseRepository<License, int>
    {
        Task<LicensesListDto> GetLicensesWithPagination(GetLicensesPaginationQuery queryParams, string userId = null, int? solutionId = null);
        Task<List<License>> GetLicensesBySolutionId(int solutionId, GetLicensesBySolutionQuery queryParams);
    }
}
