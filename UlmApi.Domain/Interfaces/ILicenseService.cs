using UlmApi.Domain.Entities;
using System.Threading.Tasks;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Models.Queries;
using FluentValidation;
using System.Collections.Generic;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Attributes;

namespace UlmApi.Domain.Interfaces
{
    public interface ILicenseService : IBaseService<License, int>
    {
        [Log(Operation.QUERY, "License", "Returning all licenses with filters.")]
        Task<LicensesListDto> GetLicensesWithPagination(GetLicensesPaginationQuery queryParams);

        [Log(Operation.QUERY, "License", "Returning licenses by solutionId.")]
        Task<List<LicenseDto>> GetLicensesBySolutionId(int solutionId, GetLicensesBySolutionQuery queryParams);
        
        [Log(Operation.QUERY, "License", "Returning license by Id.")]
        Task<LicenseDto> GetById(int id);

        [Log(Operation.CREATE, "License", "A new license was registered.")]
        Task<LicenseDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<License>
            where TInputModel : class;

        [Log(Operation.UPDATE, "License", "Archiving a license.")]
        Task<License> Archived(License license);

        [Log(Operation.UPDATE, "License", "Unarchiving a license.")]
        Task<License> Unarchived(License license);

        [Log(Operation.UPDATE, "License", "Place license to use.")]
        Task<License> PlaceInUse(License license);

        [Log(Operation.UPDATE, "License", "Unlinking the license from the application.")]
        Task<License> DisassociateApplication(License license);
    }
}
