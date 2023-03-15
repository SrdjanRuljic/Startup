using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(x => x.RecipientId).IsRequired();
            builder.Property(x => x.SenderId).IsRequired();

            builder.HasOne(x => x.Recipient)
                   .WithMany(x => x.MessagesRecived)
                   .HasForeignKey(x => x.RecipientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.MessagesSent)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}