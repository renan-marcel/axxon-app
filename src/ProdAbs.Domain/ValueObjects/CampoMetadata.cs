
using System.Collections.Generic;

namespace ProdAbs.Domain.ValueObjects
{
    public class CampoMetadata
    {
        public string Label { get; set; }
        public RegraValidacao RegraDeValidacao { get; set; }
        public string Mascara { get; set; }
    }
}
