namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public class TemplateProperty : Entity
{
    public string Key { get; set; }
    public bool AllowSearch { get; set; }
    public string Description { get; set; }
    public int TemplateId { get; set; }
    public Template Template { get; set; }
}