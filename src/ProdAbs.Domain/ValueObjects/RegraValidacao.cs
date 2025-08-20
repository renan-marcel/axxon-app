
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
        public TipoDeDados TipoDeDados { get; private set; }
        public bool Obrigatorio { get; private set; }
        public string FormatoEspecifico { get; private set; } // Ex: regex

        // Private constructor for EF Core
        private RegraValidacao()
        {
            FormatoEspecifico = string.Empty;
        }

        public RegraValidacao(TipoDeDados tipoDeDados, bool obrigatorio, string formatoEspecifico)
        {
            TipoDeDados = tipoDeDados;
            Obrigatorio = obrigatorio;
            FormatoEspecifico = formatoEspecifico;
        }
    }
}
