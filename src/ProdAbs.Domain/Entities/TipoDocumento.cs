using ProdAbs.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ProdAbs.Domain.Entities
{
    public class TipoDocumento
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<CampoMetadata> Campos { get; set; } = new();
    }
}

