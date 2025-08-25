using Microsoft.Extensions.Logging;
using ProdAbs.Application.Interfaces;

namespace ProdAbs.Infrastructure.Services;

public class SimpleAuditLogger : IAuditLogger
{
    private readonly ILoggerFactory _logger;

    public SimpleAuditLogger(ILoggerFactory logger)
    {
        _logger = logger;
    }

    public Task LogAuditAsync(string action, string entityId, string userId, string details)
    {
        var logger = _logger.CreateLogger<SimpleAuditLogger>();
        logger.LogInformation("[Audit] Action:{Action} Entity:{EntityId} User:{UserId} Details:{Details}", action, entityId,
            userId, details);
        return Task.CompletedTask;
    }
}