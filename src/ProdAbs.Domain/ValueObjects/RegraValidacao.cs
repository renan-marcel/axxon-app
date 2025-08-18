
namespace ProdAbs.Domain.ValueObjects
{
    public enum TipoDeDados
    {
        String,
        Int,
        Date
    }

    public class RegraValidacao
    {
        public TipoDeDados TipoDeDados { get; set; }
        public bool Obrigatorio { get; set; }
        public string FormatoEspecifico { get; set; } // Ex: regex
    }
}
