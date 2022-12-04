namespace Messaging.Application.IntegrationEvents.Events;

public record SendNotificationIntegrationEvent(string Content, string ToUser, string ToGroup) : IntegrationEvent;