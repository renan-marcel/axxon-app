namespace ProdAbs.Domain.ValueObjects;

public class CampoMetadata
{
    // Private constructor for EF Core
    private CampoMetadata()
    {
        Label = string.Empty;
        RegraDeValidacao = new RegraValidacao(TipoDeDados.String, false, string.Empty);
        Mascara = string.Empty;
    }

    public CampoMetadata(string label, RegraValidacao regraDeValidacao, string mascara)
    {
        Label = label;
        RegraDeValidacao = regraDeValidacao;
        Mascara = mascara;
    }

    public string Label { get; private set; }
    public RegraValidacao RegraDeValidacao { get; private set; }
    public string Mascara { get; private set; }
}