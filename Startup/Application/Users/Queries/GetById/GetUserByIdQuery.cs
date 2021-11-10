using MediatR;

namespace Application.Users.Queries.GetById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdViewModel>
    {
        public string Id { get; set; }
    }
}
