using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAbs.SharedKernel.Events;
public interface IDocumentEvent
{
    public Guid Id { get; set; }
    public Guid TipoDocumentoId { get; set; }
    public string StorageLocation { get; set; }
}
