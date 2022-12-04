using System.Text.Json;

namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public class Message : Entity, IAggregateRoot
{
    
    public string ToUser { get; set; }
    public string ToGroup { get; set; }
    public string RawData { get; set; }
    public int TemplateId { get; set; }
    public Template Template { get; set; }
}