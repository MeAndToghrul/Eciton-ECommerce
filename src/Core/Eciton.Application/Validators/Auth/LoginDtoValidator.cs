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
            .Matches(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.|\+)?[0-9a-zA-Z])*)@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$")
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Model.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
            .Must(ContainSymbol).WithMessage("Password must contain at least one special character.");
    }

    private bool ContainSymbol(string password)
    {
        return password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
