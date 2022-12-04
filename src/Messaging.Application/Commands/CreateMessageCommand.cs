namespace Messaging.Application.Commands;

public class CreateMessageCommand : IRequest<bool>
{
    public int TemplateId { get; set; }
    public string ToUser { get; set; }
    public dynamic RawData { get; set; }
}
