namespace ProdAbs.Domain.ValueObjects;

public class MetadadoDocumento
{
    public long TamanhoEmBytes { get; set; }
    public string HashTipo { get; set; } = string.Empty;
    public string HashValor { get; set; } = string.Empty;
    public string NomeArquivoOriginal { get; set; } = string.Empty;
    public string Formato { get; set; } = string.Empty;
    public int Versao { get; set; }
}