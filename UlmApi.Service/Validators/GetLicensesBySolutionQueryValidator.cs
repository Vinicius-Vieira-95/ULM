using FluentValidation;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Service.Validators
{
    public class GetLicensesBySolutionQueryValidator : AbstractValidator<GetLicensesBySolutionQuery>
    {
        public GetLicensesBySolutionQueryValidator()
        {
            RuleForEach(p => p.Status)
                .IsInEnum()
                .WithMessage("Status value is invalid.");
        }
    }
}
