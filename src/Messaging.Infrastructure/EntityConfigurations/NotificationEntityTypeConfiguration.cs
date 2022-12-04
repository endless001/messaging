using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.Infrastructure.EntityConfigurations;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName("id");

        builder.Property(n => n.MessageId)
            .HasColumnName("message_id");

        builder.Property(n => n.TemplateId)
            .HasColumnName("template_id");
        
        builder.Property(n => n.TemplateName)
            .HasColumnName("template_name");

        builder.Property(n => n.Title)
            .HasColumnName("title");

        builder.Property(n => n.Content)
            .HasColumnName("content");

        builder.Property(m => m.RawData)
            .HasColumnType("jsonb")
            .HasColumnName("raw_data")
            .IsRequired(false);

        builder.Property(m => m.Read)
            .HasColumnName("read");

        builder.Property(m => m.Created)
            .HasColumnName("created");
    }
}