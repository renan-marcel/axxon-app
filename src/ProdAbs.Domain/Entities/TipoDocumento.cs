using ProdAbs.Domain.ValueObjects;

namespace ProdAbs.Domain.Entities
{
    public class TipoDocumento
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<CampoMetadata> Campos { get; set; } = new List<CampoMetadata>();
    }
}
