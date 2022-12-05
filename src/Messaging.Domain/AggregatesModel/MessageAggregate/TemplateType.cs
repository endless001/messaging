namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public class TemplateType : Enumeration
{

    public static TemplateType Notification = new(1, nameof(Notification));
    public static TemplateType SMS = new(2, nameof(SMS));
    public static TemplateType Email = new(3, nameof(Email));

    public TemplateType(int id, string name) : base(id, name)
    {
    }
}