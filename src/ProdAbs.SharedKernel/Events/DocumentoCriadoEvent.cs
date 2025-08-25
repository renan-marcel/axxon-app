namespace ProdAbs.SharedKernel.Events;

public class DocumentoCriadoEvent : IDocumentoCriadoEvent
{
    public string HashValor { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public Guid TipoDocumentoId { get; set; }
    public string StorageLocation { get; set; } = string.Empty;
    public long TamanhoEmBytes { get; set; }
}