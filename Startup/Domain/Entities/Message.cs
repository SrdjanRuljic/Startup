using Domain.Entities.Identity;
using System;

namespace Domain.Entities
{
    public class Message
    {
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public long Id { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public AppUser Recipient { get; set; }
        public bool RecipientDeleted { get; set; }
        public string RecipientId { get; set; }
        public AppUser Sender { get; set; }
        public bool SenderDeleted { get; set; }
        public string SenderId { get; set; }
    }
}