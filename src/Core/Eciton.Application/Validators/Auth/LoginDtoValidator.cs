using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using FluentValidation;

namespace Eciton.Application.Validators.Auth;
public class LoginDtoValidator : AbstractValidator<LoginUserCommand>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Model.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Model.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

    }    

}
