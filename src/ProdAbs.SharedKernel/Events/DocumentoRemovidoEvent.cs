namespace ProdAbs.SharedKernel.Events;

public class DocumentoRemovidoEvent
{
    public Guid Id { get; set; }
    public string StorageLocation { get; set; } = string.Empty;
    public long TamanhoEmBytes { get; set; }
}