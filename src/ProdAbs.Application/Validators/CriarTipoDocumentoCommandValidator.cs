using FluentValidation;
using ProdAbs.Application.Features.TiposDocumento.Commands;

namespace ProdAbs.Application.Validators
{
    public class CriarTipoDocumentoCommandValidator : AbstractValidator<CriarTipoDocumentoCommand>
    {
        public CriarTipoDocumentoCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do tipo de documento é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.");

            RuleFor(x => x.Campos)
                .NotEmpty().WithMessage("A lista de campos não pode ser vazia.");
        }
    }
}
