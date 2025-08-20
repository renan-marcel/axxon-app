using ProdAbs.SharedKernel.BaseClasses;

namespace ProdAbs.Domain.Entities;

public class Prontuario : AggregateRoot<Guid>
{
    // Private constructor for EF Core
    private Prontuario(Guid id) : base(id)
    {
        IdentificadorEntidade = string.Empty;
        TipoProntuario = string.Empty;
    }

    public Prontuario(Guid id, string identificadorEntidade, string tipoProntuario) : base(id)
    {
        IdentificadorEntidade = identificadorEntidade;
        TipoProntuario = tipoProntuario;
    }

    public string IdentificadorEntidade { get; private set; }
    public string TipoProntuario { get; private set; }
    public List<Guid> DocumentoIds { get; } = new();

    public void AdicionarDocumento(Guid documentoId)
    {
        if (!DocumentoIds.Contains(documentoId)) DocumentoIds.Add(documentoId);
    }
}