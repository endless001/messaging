using EventBus.Abstractions;
using Messaging.SignalrHub.IntegrationEvents.Events;

namespace Messaging.SignalrHub.IntegrationEvents.EventHandling;

public class SendEmailIntegrationEventHandler : IIntegrationEventHandler<SendEmailIntegrationEvent>
{
    public Task Handle(SendEmailIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}