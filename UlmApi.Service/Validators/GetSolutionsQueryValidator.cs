using FluentValidation;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Service.Validators
{
    public class GetSolutionsQueryValidator : AbstractValidator<GetSolutionsQuery>
    {
        public GetSolutionsQueryValidator()
        {
            RuleFor(p => p.Limit)
                .GreaterThanOrEqualTo(1)
                .WithMessage("The Limit must be greater than 1.");
            
            RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The page must be greater than or equal to 0.");
        }
    }
}
