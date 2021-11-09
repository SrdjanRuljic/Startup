using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendConfirmationEmailAsync(Message message);
        Task SendPasswordEmailAsync(Message message);
    }
}
