using Eciton.Application.Commands.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Validators.Auth
{
    public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestCommand>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .Matches(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.|\+)?[0-9a-zA-Z])*)@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$")
                .WithMessage("Invalid email format.");
        }
    }
}
