using EventBus.Events;

namespace Messaging.SignalrHub.IntegrationEvents.Events;

public record SendNotificationIntegrationEvent : IntegrationEvent
{
    public string Content { get; }
    public string ToUser { get; }
    public string ToGroup { get; }

    public SendNotificationIntegrationEvent(string content, string toUser, string toGroup)
    {
        Content = content;
        ToUser = toUser;
        ToGroup = toGroup;
    }
}