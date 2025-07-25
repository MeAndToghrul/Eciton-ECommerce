using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Eciton.Application.Validators.Auth;
public class RegisterDtoValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Model.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Model.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(50).WithMessage("Surname must not exceed 50 characters.");

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

        RuleFor(x => x.Model.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password.")
            .Equal(x => x.Model.Password).WithMessage("Passwords do not match.");
    }

    private bool ContainSymbol(string password)
    {
        var specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>?/`~";
        return password.Any(ch => specialChars.Contains(ch));
    }

}
