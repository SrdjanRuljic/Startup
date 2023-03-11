using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;
using System.Linq;

namespace Application.Users.Queries.GetById
{
    public class GetUserByIdViewModel : IMapFrom<AppUser>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, GetUserByIdViewModel>()
                   .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.Role.Name)));
        }
    }
}