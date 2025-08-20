namespace ProdAbs.Application.Interfaces;

public interface IAuditLogger
{
    Task LogAuditAsync(string action, string entityId, string userId, string details);
}