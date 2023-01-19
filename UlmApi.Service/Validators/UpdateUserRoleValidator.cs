using FluentValidation;
using UlmApi.Domain.Models;

namespace UlmApi.Service.Validators
{
    public class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleModel>
    {
        public UpdateUserRoleValidator()
        {
            RuleFor(p => p.Role)
                .NotNull().WithMessage("Role is required")
                .IsInEnum().WithMessage("Role value is invalid.");
        }
    }
}
