using Microsoft.Extensions.Logging;
using ProdAbs.Application.Interfaces; 

namespace ProdAbs.Infrastructure.Services;

public class LocalEmailNotifier : IEmailNotifier
{
    private readonly ILoggerFactory _logger;

    public LocalEmailNotifier(ILoggerFactory logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Minimal local implementation: write to log
        var logger = _logger.CreateLogger<LocalEmailNotifier>();
        logger.LogInformation("[Email] To: {To} Subject: {Subject} Body: {Body}", to, subject, body);
        return Task.CompletedTask;
    }
}