using System.Collections.Generic;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Interfaces
{
    public interface IDashboardService
    {
        Task<CountLicensesExpiringDto> GetCountLicensesExpiring();
        Task<CountRequestsPendingAprovalDto> GetCountRequestsPendingAproval();
        Task<CountLicensesDto> GetCountLicensesInUse();
        Task<List<ApplicationLicenseUsageDto>> GetApplicationsLicenseUsage(int limit);
        Task<LicensePerSolutionListDto> GetLicensesPerSolution(int limit);
        Task<List<CostLicensePerMothDto>> GetCostsPerMonth();
    }
}
