using ProdAbs.Application.Interfaces;
using Serilog;

namespace ProdAbs.Infrastructure.Services;

public class SimpleAuditLogger : IAuditLogger
{
    public Task LogAuditAsync(string action, string entityId, string userId, string details)
    {
        Log.Information("[Audit] Action:{Action} Entity:{EntityId} User:{UserId} Details:{Details}", action, entityId,
            userId, details);
        return Task.CompletedTask;
    }
}