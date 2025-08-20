namespace ProdAbs.Application.DTOs;

public class TipoDocumentoDetalhesDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<string> Campos { get; set; } = new();
}