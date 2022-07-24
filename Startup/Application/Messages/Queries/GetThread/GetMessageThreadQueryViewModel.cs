using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Messages.Queries.GetThread
{
    public class GetMessageThreadQueryViewModel : IMapFrom<Message>
    {
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public long Id { get; set; }
        public DateTime MessageSent { get; set; }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, GetMessageThreadQueryViewModel>();
            profile.CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}