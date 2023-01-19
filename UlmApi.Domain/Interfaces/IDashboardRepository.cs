using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities.Enums;

namespace UlmApi.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<CountLicensesExpiringDto> GetCountLicensesExpiring(int? solutionId = null);
        Task<CountRequestsPendingAprovalDto> GetCountRequestsPendingAproval(int? solutionId = null);
        Task<CountLicensesDto> GetCountLicensesInUse(int? solutionId = null);
        Task<int> GetCountLicensesByApplicationId(int applicationId);
        Task<int> GetCountLicensesBySolutionId(int solutionId, LicenseStatus status);
        Task<double> GetCostPerYearMoth(int year, int month);
    }
}
