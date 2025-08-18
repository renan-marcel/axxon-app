
namespace ProdAbs.Domain.ValueObjects
{
    public class MetadadoDocumento
    {
        public long TamanhoEmBytes { get; set; }
        public string HashTipo { get; set; }
        public string HashValor { get; set; }
        public string NomeArquivoOriginal { get; set; }
        public string Formato { get; set; }
        public int Versao { get; set; }
    }
}
