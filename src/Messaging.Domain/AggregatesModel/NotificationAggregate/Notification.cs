namespace Messaging.Domain.AggregatesModel.NotificationAggregate;

public class Notification : Entity, IAggregateRoot
{
    public int MessageId { get; set; }
    public int TemplateId { get; set; }
    public string TemplateName { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string RawData { get; set; }
    public bool Read { get; set; }
    public DateTime Created { get; set; }
}