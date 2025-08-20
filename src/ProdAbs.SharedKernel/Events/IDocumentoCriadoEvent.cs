namespace ProdAbs.SharedKernel.Events;

public interface IDocumentoCriadoEvent : IDocumentEvent
{
    public long TamanhoEmBytes { get; set; }
}