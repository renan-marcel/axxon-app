using MassTransit;
using ProdAbs.SharedKernel.Events;
using ProdAbs.Application.Interfaces;

namespace ProdAbs.Infrastructure.Messaging
{
    public class DocumentoCriadoConsumer : IConsumer<IDocumentoCriadoEvent>
    {
        private readonly IAuditLogger _auditLogger;

        public DocumentoCriadoConsumer(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public async Task Consume(ConsumeContext<IDocumentoCriadoEvent> context)
        {
            var evt = context.Message;

            // Minimal projection/audit behaviour: log via audit logger
            await _auditLogger.LogAuditAsync("DocumentoCriado", evt.Id.ToString(), "system", $"Storage:{evt.StorageLocation} Size:{evt.TamanhoEmBytes}");
        }
    }
}
