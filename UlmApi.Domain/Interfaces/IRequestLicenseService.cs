using UlmApi.Domain.Entities;
using System.Threading.Tasks;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Models.Queries;
using FluentValidation;
using UlmApi.Domain.Attributes;
using UlmApi.Domain.Models;
using System.Collections.Generic;

namespace UlmApi.Domain.Interfaces
{
    public interface IRequestLicenseService : IBaseService<RequestLicense, int>
    {
        [Log(Operation.UPDATE, "RequestLicense", "The request is in analysis.")]
        Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, RequestLicenseStatus status);

        [Log(Operation.UPDATE, "RequestLicense", "The request was accepted.")]
        Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, License license, RequestLicenseStatus status);

        [Log(Operation.UPDATE, "RequestLicense", "The request was rejected.")]
        Task<RequestLicenseDto> ChangeStatusRequest(RequestLicense requestLicense, RequestLicenseStatus status, string justificationForDeny);
        
        [Log(Operation.QUERY, "RequestLicense", "Returning all requests with filters.")]
        Task<RequestLicensesListDto> GetRequestLicensesWithPagination(GetRequestLicensesPaginationQuery queryParams);
        
        [Log(Operation.QUERY, "RequestLicense", "Returning request by Id.")]
        Task<RequestLicenseDto> GetById(int id);

        [Log(Operation.QUERY, "RequestLicense", "Returning available usage times.")]
        List<GenericEnumModel> GetUsageTimeEnum();
        
        [Log(Operation.QUERY, "RequestLicense", "Returning available requests reasons.")]
        List<GenericEnumModel> GetRequisitionReasons();
        
        Task<RequestLicense> UpdateIaFields(UpdateIaFieldsModel model);

        [Log(Operation.CREATE, "RequestLicense", "A new request was registered")]  
        Task<RequestLicenseDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<RequestLicense>
            where TInputModel : class;
    }
}
