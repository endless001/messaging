namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public class TemplateType : Enumeration
{
    public static TemplateType Email = new(1, nameof(Email));
    public static TemplateType SMS = new(2, nameof(SMS));
    public static TemplateType Notification = new(3, nameof(Notification));
    
    public TemplateType(int id, string name) : base(id, name)
    {
    }
}