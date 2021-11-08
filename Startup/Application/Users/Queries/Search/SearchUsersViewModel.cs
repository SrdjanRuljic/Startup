using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Users.Queries.Search
{
    public class SearchUsersViewModel : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, SearchUsersViewModel>();
        }
    }
}
