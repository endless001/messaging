using EventBus.Events;

namespace Messaging.SignalrHub.IntegrationEvents.Events;

public record SendEmailIntegrationEvent(int MessageId) : IntegrationEvent;