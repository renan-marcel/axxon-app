namespace ProdAbs.Domain.Entities
{
    public class Prontuario
    {
        public Guid Id { get; set; }
        public string IdentificadorEntidade { get; set; }
        public string TipoProntuario { get; set; }
        public List<Guid> DocumentoIds { get; set; } = new List<Guid>();
    }
}
