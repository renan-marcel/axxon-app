using FluentValidation;
using ProdAbs.Domain.ValueObjects;

namespace ProdAbs.Application.Validators;

public class CampoMetadataValidator : AbstractValidator<CampoMetadata>
{
    public CampoMetadataValidator()
    {
        RuleFor(x => x.Label).NotEmpty().WithMessage("Label do campo é obrigatório.");
        RuleFor(x => x.RegraDeValidacao).NotNull().WithMessage("Regra de validação é obrigatória.");
        RuleFor(x => x.RegraDeValidacao.TipoDeDados).IsInEnum().WithMessage("Tipo de dados inválido.");
        RuleFor(x => x.RegraDeValidacao.FormatoEspecifico)
            .Matches("^[a-zA-Z0-9]*$") // Exemplo: apenas alfanumérico
            .When(x => !string.IsNullOrEmpty(x.RegraDeValidacao.FormatoEspecifico))
            .WithMessage("Formato específico inválido.");
    }
}