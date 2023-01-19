using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface IRequestLicenseRepository : IBaseRepository<RequestLicense, int>
    {
        Task<RequestLicensesListDto> GetRequestLicensesBySolutionId(GetRequestLicensesPaginationQuery queryParams, int solutionId);
        Task<RequestLicensesListDto> GetRequestLicensesByRequesterId(GetRequestLicensesPaginationQuery queryParams, string requesterId);
        Task<RequestLicensesListDto> GetAllRequests(GetRequestLicensesPaginationQuery queryParams);
    }
}