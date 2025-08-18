using System.Threading.Tasks;

namespace ProdAbs.Application.Interfaces
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}