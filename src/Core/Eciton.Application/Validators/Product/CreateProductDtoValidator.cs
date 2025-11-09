using Eciton.Application.Commands.Product;
using Eciton.Application.DTOs.Product;
using FluentValidation;

namespace Eciton.Application.Validators.Product
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Model.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(150).WithMessage("Brand cannot exceed 150 characters.");

            RuleFor(x => x.Model.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Model.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(100).WithMessage("Model cannot exceed 100 characters.");

            RuleFor(x => x.Model.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity cannot be negative.");

            RuleFor(x => x.Model.ProductImage)
                .NotNull().WithMessage("Product image is required.");

            RuleFor(x => x.Model.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
