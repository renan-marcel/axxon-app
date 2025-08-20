using ProdAbs.SharedKernel.BaseClasses;

namespace ProdAbs.Domain.Entities;

public class Documento : AggregateRoot<Guid>
{
    // Private constructor for EF Core
    private Documento(Guid id) : base(id)
    {
        TipoDeDocumentoId = Guid.Empty;
        StorageLocation = string.Empty;
        HashTipo = string.Empty;
        HashValor = string.Empty;
        NomeArquivoOriginal = string.Empty;
        Formato = string.Empty;
        DicionarioDeCamposValores = new Dictionary<string, string>();
    }

    public Documento(
        Guid id,
        Guid tipoDeDocumentoId,
        string storageLocation,
        long tamanhoEmBytes,
        string hashTipo,
        string hashValor,
        string nomeArquivoOriginal,
        string formato,
        IReadOnlyDictionary<string, string> dicionarioDeCamposValores,
        int versao = 1) : base(id)
    {
        TipoDeDocumentoId = tipoDeDocumentoId;
        StorageLocation = storageLocation;
        TamanhoEmBytes = tamanhoEmBytes;
        HashTipo = hashTipo;
        HashValor = hashValor;
        NomeArquivoOriginal = nomeArquivoOriginal;
        Formato = formato;
        DicionarioDeCamposValores = dicionarioDeCamposValores;
        Versao = versao;
    }

    public Guid TipoDeDocumentoId { get; private set; }
    public string StorageLocation { get; private set; }
    public long TamanhoEmBytes { get; private set; }
    public string HashTipo { get; private set; }
    public string HashValor { get; private set; }
    public string NomeArquivoOriginal { get; private set; }
    public string Formato { get; private set; }
    public int Versao { get; private set; }
    public IReadOnlyDictionary<string, string> DicionarioDeCamposValores { get; private set; }
}