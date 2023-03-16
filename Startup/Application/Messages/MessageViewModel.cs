using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;

namespace Application.Messages
{
    public class MessageViewModel : IMapFrom<Message>
    {
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public long Id { get; set; }
        public DateTime MessageSent { get; set; }
        public string RecipientId { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string RecipientUserName { get; set; }
        public string SenderId { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string SenderUserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, MessageViewModel>()
                   .ForMember(dest => dest.RecipientUserName, opt => opt.MapFrom(src => src.Recipient.UserName))
                   .ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => src.Sender.UserName));

            profile.CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}