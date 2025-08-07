using Eciton.Application.Commands.CategoryField;
using FluentValidation;
namespace Eciton.Application.Validators.CategoryField;
public class CreateCategoryFieldDtoValidator : AbstractValidator<CreateCategoryFieldCommand>
{
    public CreateCategoryFieldDtoValidator()
    {
        RuleFor(x => x.Model.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.Model.FieldName)
            .NotEmpty().WithMessage("FieldName is required.")
            .MaximumLength(100).WithMessage("FieldName cannot exceed 100 characters.");

        RuleFor(x => x.Model.DataType)
            .NotEmpty().WithMessage("DataType is required.")
            .Must(dt => new[] { "string", "int", "bool", "double", "decimal", "datetime" }
            .Contains(dt.ToLower()))
            .WithMessage("DataType must be one of the following: string, int, bool, double, decimal, datetime.");
    }
}
