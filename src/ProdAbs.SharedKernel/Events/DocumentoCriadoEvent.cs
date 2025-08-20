namespace ProdAbs.SharedKernel.Events
{
    public class DocumentoCriadoEvent:  IDocumentoCriadoEvent
    {
        public Guid Id { get; set; }
        public Guid TipoDocumentoId { get; set; }
        public string StorageLocation { get; set; }
        public long TamanhoEmBytes { get; set; }
        public string HashValor { get; set; }
    }
}
