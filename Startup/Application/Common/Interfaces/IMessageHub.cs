using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMessageHub
    {
        Task SendMessage();
    }
}