using Eciton.Application.Commands.Auth;
using FluentValidation;
namespace Eciton.Application.Validators.Auth;
public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.Model.OldPassword)
            .NotEmpty().WithMessage("Old password is required.");

        RuleFor(x => x.Model.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
            .Must(ContainSymbol).WithMessage("New password must contain at least one special character.");

        RuleFor(x => x.Model.ConfirmNewPassword)
            .NotEmpty().WithMessage("Please confirm your new password.")
            .Equal(x => x.Model.NewPassword).WithMessage("Passwords do not match.");
    }

    private bool ContainSymbol(string password)
    {
        var specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>?/`~";
        return password.Any(ch => specialChars.Contains(ch));
    }
}
