using ProdAbs.Application.Interfaces;
using Serilog;

namespace ProdAbs.Infrastructure.Services
{
    public class LocalEmailNotifier : IEmailNotifier
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            // Minimal local implementation: write to log
            Serilog.Log.Information("[Email] To: {To} Subject: {Subject} Body: {Body}", to, subject, body);
            return Task.CompletedTask;
        }
    }
}
