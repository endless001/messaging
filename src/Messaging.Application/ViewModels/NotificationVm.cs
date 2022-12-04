namespace Messaging.Application.ViewModels;

public class NotificationVm
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string TemplateName { get; set; }
    public bool Read { get; set; }
    public DateTime Created { get; set; }
}