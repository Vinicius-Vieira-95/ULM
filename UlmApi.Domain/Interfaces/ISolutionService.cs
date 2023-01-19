using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using UlmApi.Domain.Attributes;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface ISolutionService : IBaseService<Solution, int>
    {

        [Log(Operation.QUERY, "Solution", "Returning solutions.")]
        Task<List<SolutionDto>> GetSolutions();

        [Log(Operation.QUERY, "Solution", "Returning all solutions with filters.")]
        Task<SolutionsListDto> GetSolutionsWithPagination(GetSolutionsQuery queryParams);
        
        [Log(Operation.QUERY, "Solution", "Returning solution by Id.")]
        Task<SolutionDto> GetSolutionById(int id);

        [Log(Operation.CREATE, "Solution", "A new solution was registered")]  
        Task<SolutionDto> Create<TInputModel, TValidator>(TInputModel inputModel)
            where TValidator : AbstractValidator<Solution>
            where TInputModel : class;
    }
}
