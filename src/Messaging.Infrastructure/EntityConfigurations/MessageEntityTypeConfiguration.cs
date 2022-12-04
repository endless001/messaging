namespace Messaging.Infrastructure.EntityConfigurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        builder.Property(m => m.RawData)
            .HasColumnType("jsonb")
            .IsRequired(false);
    }
}