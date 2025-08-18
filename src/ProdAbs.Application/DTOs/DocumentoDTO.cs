namespace ProdAbs.Application.DTOs;

public class DocumentoDTO
{
    public Guid Id { get; set; }
    public Guid TipoDeDocumentoId { get; set; }
    public string NomeArquivoOriginal { get; set; }
    public string Formato { get; set; }
    public long TamanhoEmBytes { get; set; }
    public int Versao { get; set; }
    public string Status { get; set; }
    public IReadOnlyDictionary<string, string> DicionarioDeCamposValores { get; set; }
}
