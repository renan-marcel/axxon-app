using System;
using System.Collections.Generic;
using ProdAbs.Domain.ValueObjects;

namespace ProdAbs.Application.DTOs
{
    public class TipoDocumentoDetalhesDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<CampoMetadata> Campos { get; set; }
    }
}