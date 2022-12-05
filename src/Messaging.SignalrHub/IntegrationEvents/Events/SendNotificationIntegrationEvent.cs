using EventBus.Events;

namespace Messaging.SignalrHub.IntegrationEvents.Events;

public record SendNotificationIntegrationEvent(string Content, string ToUser, string ToGroup) : IntegrationEvent;