using FluentValidation;
using UlmApi.Domain.Models;

namespace UlmApi.Service.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(r => r.Password)
                .Equal(r => r.ConfirmPassword).WithMessage("Passwords do not match.");
            
            RuleFor(r => r.ConfirmPassword)
                .Equal(r => r.Password).WithMessage("Passwords do not match.");
        }
    }
}