using ProdAbs.Domain.ValueObjects;
using ProdAbs.SharedKernel.BaseClasses;
using System;
using System.Collections.Generic;

namespace ProdAbs.Domain.Entities
{
    public class TipoDocumento : AggregateRoot<Guid>
    {
        public string Nome { get; private set; }
        public List<CampoMetadata> Campos { get; private set; } = new();

        // Private constructor for EF Core
        private TipoDocumento(Guid id) : base(id) 
        {
            Nome = string.Empty;
        }

        public TipoDocumento(Guid id, string nome, List<CampoMetadata> campos) : base(id)
        {
            Nome = nome;
            Campos = campos;
        }
    }
}

