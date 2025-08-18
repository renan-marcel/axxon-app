namespace ProdAbs.SharedKernel.Events
{
    public class DocumentoRemovidoEvent
    {
        public Guid Id { get; set; }
        public string StorageLocation { get; set; }
        public long TamanhoEmBytes { get; set; }
    }
}
