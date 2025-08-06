using Eciton.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eciton.Application.Validators.Category
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.CategoryImage)
                .NotNull().WithMessage("Category image is required.")
                .Must(file => file.Length > 0).WithMessage("Category image must not be empty.")
                .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                .WithMessage("Category image must be a JPEG or PNG file.");
        }
    }
}
