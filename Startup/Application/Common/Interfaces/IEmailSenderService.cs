using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(Message message);
    }
}
