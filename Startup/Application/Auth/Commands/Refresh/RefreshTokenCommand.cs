using MediatR;

namespace Application.Auth.Commands.Refresh
{
    public class RefreshTokenCommand : IRequest<object>
    {
        public string RefreshToken { get; set; }
    }
}