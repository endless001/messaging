namespace Messaging.Application.IntegrationEvents.Events;


public record SendEmailIntegrationEvent(int MessageId) : IntegrationEvent;