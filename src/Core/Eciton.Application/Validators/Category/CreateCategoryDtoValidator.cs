using Eciton.Application.Commands.Category;
using FluentValidation;

namespace Eciton.Application.Validators.Category
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Model.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Model.CategoryImage)
                .NotNull().WithMessage("Category image is required.")
                .Must(file => file != null && file.Length > 0).WithMessage("Category image must not be empty.")
                .Must(file => file != null && (file.ContentType == "image/jpeg" || file.ContentType == "image/png"))
                .WithMessage("Category image must be a JPEG or PNG file.");
        }
    }
}
