namespace ProdAbs.SharedKernel.Events;

public interface IDocumentEvent
{
    public Guid Id { get; set; }
    public Guid TipoDocumentoId { get; set; }
    public string StorageLocation { get; set; }
}