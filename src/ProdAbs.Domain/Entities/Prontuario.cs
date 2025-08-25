using ProdAbs.SharedKernel.BaseClasses;
using ProdAbs.SharedKernel.Interfaces;

namespace ProdAbs.Domain.Entities;

public class Prontuario : AggregateRoot<Guid>, IHasCreatedDate
{
    // Private constructor for EF Core
    private Prontuario(Guid id) : base(id)
    {
        IdentificadorEntidade = string.Empty;
        TipoProntuario = string.Empty;
    }

    public Prontuario(Guid id, string identificadorEntidade, string tipoProntuario, DateTimeOffset createdDate) : base(id)
    {
        IdentificadorEntidade = identificadorEntidade;
        TipoProntuario = tipoProntuario;
        CreatedDate = createdDate;
    }

    public string IdentificadorEntidade { get; private set; }
    public string TipoProntuario { get; private set; }
    public List<Guid> DocumentoIds { get; } = new();

    public void AdicionarDocumento(Guid documentoId)
    {
        if (!DocumentoIds.Contains(documentoId)) DocumentoIds.Add(documentoId);
    }

    public DateTimeOffset CreatedDate { get; }
}