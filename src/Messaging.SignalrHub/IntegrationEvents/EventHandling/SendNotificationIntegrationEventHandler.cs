using EventBus.Abstractions;
using Messaging.SignalrHub.IntegrationEvents.Events;
using Microsoft.AspNetCore.SignalR;

namespace Messaging.SignalrHub.IntegrationEvents.EventHandling;

public class SendNotificationIntegrationEventHandler : IIntegrationEventHandler<SendNotificationIntegrationEvent>
{
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly ILogger<SendNotificationIntegrationEventHandler> _logger;

    public SendNotificationIntegrationEventHandler(IHubContext<NotificationsHub> hubContext,
        ILogger<SendNotificationIntegrationEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(SendNotificationIntegrationEvent @event)
    {
        await _hubContext.Clients
            .Group(@event.ToUser)
            .SendAsync("SendNotification", new { Content = @event.Content });
    }
}