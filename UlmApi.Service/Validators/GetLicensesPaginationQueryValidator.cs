using FluentValidation;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Service.Validators
{
    public class GetLicensesPaginationQueryValidator : AbstractValidator<GetLicensesPaginationQuery>
    {
        public GetLicensesPaginationQueryValidator()
        {
            RuleFor(p => p.Limit)
                .GreaterThanOrEqualTo(1)
                .WithMessage("The Limit must be greater than 1.");
            
            RuleFor(p => p.Page)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The page must be greater than or equal to 0.");
            
            RuleForEach(p => p.Status)
                .IsInEnum()
                .WithMessage("Status value is invalid.");
        }
    }
}
