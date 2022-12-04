namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public class Template : Entity
{
    public string Name { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int TemplateTypeId { get; set; }
    public TemplateType TemplateType { get; set; }
    public List<TemplateProperty> TemplateProperties { get; set; }
}