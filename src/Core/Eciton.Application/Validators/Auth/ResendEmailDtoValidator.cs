using Eciton.Application.Commands.Auth;
using FluentValidation;
namespace Eciton.Application.Validators.Auth;
public class ResendEmailDtoValidator : AbstractValidator<ResendEmailVerificationCommand>
{
    public ResendEmailDtoValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
            .Matches(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.|\+)?[0-9a-zA-Z])*)@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$")
            .WithMessage("Invalid email format.");
    }
}
