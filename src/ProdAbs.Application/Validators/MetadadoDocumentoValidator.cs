using FluentValidation;
using ProdAbs.Domain.ValueObjects;

namespace ProdAbs.Application.Validators;

public class MetadadoDocumentoValidator : AbstractValidator<MetadadoDocumento>
{
    public MetadadoDocumentoValidator()
    {
        RuleFor(x => x.TamanhoEmBytes).GreaterThan(0).WithMessage("Tamanho do documento deve ser maior que zero.");
        RuleFor(x => x.HashTipo).NotEmpty().WithMessage("Tipo de hash é obrigatório.");
        RuleFor(x => x.HashValor).NotEmpty().WithMessage("Valor do hash é obrigatório.");
        RuleFor(x => x.NomeArquivoOriginal).NotEmpty().WithMessage("Nome do arquivo original é obrigatório.");
        RuleFor(x => x.Formato).NotEmpty().WithMessage("Formato do arquivo é obrigatório.");
        RuleFor(x => x.Versao).GreaterThanOrEqualTo(1).WithMessage("Versão deve ser maior ou igual a 1.");
    }
}