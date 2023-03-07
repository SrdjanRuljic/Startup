using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Users.Queries.GetLoggedIn
{
    public class GetLoggedInUserViewModel : IMapFrom<AppUser>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, GetLoggedInUserViewModel>();
        }
    }
}