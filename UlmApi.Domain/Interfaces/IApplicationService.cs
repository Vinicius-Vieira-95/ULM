using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using UlmApi.Domain.Attributes;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface IApplicationService : IBaseService<Application, int>
    {
        [Log(Operation.QUERY, "Application", "Returning all applications with filters.")]
        Task<ApplicationListDto> GetApplicationWithPagination(GetApplicationPaginationQuery queryParams);
        
        [Log(Operation.QUERY, "Application", "Returning all applications.")]
        Task<List<ApplicationDto>> GetAllApplications();
        
        [Log(Operation.QUERY, "Application", "Returning application by Id.")]
        Task<ApplicationDto> GetApplicationById(int id);
        
        [Log(Operation.CREATE, "Application", "A new Application was registered.")]
        Task<ApplicationDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<Application>
            where TInputModel : class;
    }
}
