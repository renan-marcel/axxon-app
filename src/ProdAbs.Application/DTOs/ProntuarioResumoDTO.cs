using System;
using System.Collections.Generic;

namespace ProdAbs.Application.DTOs
{
    public class ProntuarioResumoDTO
    {
        public Guid Id { get; set; }
        public string IdentificadorEntidade { get; set; }
        public string TipoProntuario { get; set; }
        public List<Guid> DocumentoIds { get; set; }
    }
}