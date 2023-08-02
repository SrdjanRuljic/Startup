using Application.Common.Behaviours;
using Application.Mappings;
using AutoMapper;
using Domain.Entities.Identity;

namespace Application.Messages.Queries.SearchInterlocutors
{
    public class SearchInterlocutorsViewModel : IMapFrom<AppUser>
    {
        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return FullNameResolver.Resolve(FirstName, LastName, UserName);
            }
        }

        public string Id { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, SearchInterlocutorsViewModel>();
        }
    }
}