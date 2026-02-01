using Carglass.TechnicalAssessment.Backend.Dtos.Extensions;
using Carglass.TechnicalAssessment.Backend.Dtos.Resources;
using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.Dtos.Clients;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
	private List<string> NumberDocumentValied = new List<string> { "nif"};
    public ClientDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(string.Format(Messages.IdRequired, "Client"));

        RuleFor(x => x.DocType)
            .NotEmpty()
            .WithMessage(Messages.DocTypeRequired)
            .MaximumLength(25)
            .WithMessage(string.Format(Messages.LengthMaxRequired, "tipo de documento", 25));

        RuleFor(x => x.DocNum)
            .NotEmpty()
            .WithMessage(Messages.DocNumRequired)
            .MaximumLength(12)
            .WithMessage(string.Format(Messages.LengthMaxRequired, "número de documento", 12))
			.MustValidNumberDocument()
			.When(p => NumberDocumentValied.Contains(p.DocType.ToLower()));
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(string.Format(Messages.Required,"email"))
            .EmailAddress()
            .WithMessage(Messages.EmailInvalidFormat);
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(string.Format(Messages.Required, "nombre"))
            .MinimumLength(3)
            .WithMessage(string.Format(Messages.LengthMinRequired, "nombre", 3));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(string.Format(Messages.Required, "apellido"))
            .MinimumLength(3)
            .WithMessage(string.Format(Messages.LengthMinRequired, "apellido", 3));

        RuleFor(x => x.Phone)
            .MinimumLength(9)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage(string.Format(Messages.LengthMinRequired, "teléfono", 9));
    }
}