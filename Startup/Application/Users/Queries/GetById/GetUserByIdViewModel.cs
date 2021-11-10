using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Users.Queries.GetById
{
    public class GetUserByIdViewModel : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, GetUserByIdViewModel>();
        }
    }
}
