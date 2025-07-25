using Eciton.Application.Commands.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Validators.Auth
{
    public class ResetPasswordDtoValidator : AbstractValidator<ConfirmResetPasswordCommand>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x=>x.Model.Token)
                .NotEmpty().WithMessage("Token is required.")
                .MinimumLength(150).WithMessage("Token must be at least 150 characters long.")
                .MaximumLength(1000).WithMessage("Token must not exceed 1000 characters.");


            RuleFor(x => x.Model.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
                .Must(ContainSymbol).WithMessage("New password must contain at least one special character.");


            RuleFor(x => x.Model.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.Model.NewPassword).WithMessage("Passwords do not match.");
        }
        private bool ContainSymbol(string password)
        {
            var specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>?/`~";
            return password.Any(ch => specialChars.Contains(ch));
        }
    }
}
