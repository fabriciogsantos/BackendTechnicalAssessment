using Carglass.TechnicalAssessment.Backend.Dtos.Resources;
using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.Dtos.Products;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(string.Format(Messages.IdRequired, "Product"));

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(Messages.ProductNameRequired);

        RuleFor(x => x.Type)
            .GreaterThanOrEqualTo(1)
            .WithMessage(string.Format(string.Format(Messages.GreaterOrEqualOneType, "tipo de producto")));

        RuleFor(x => x.NumTerminal)
            .GreaterThanOrEqualTo(1)
            .WithMessage(string.Format(Messages.GreaterOrEqualOneType,"número de Terminal"));
    }
}