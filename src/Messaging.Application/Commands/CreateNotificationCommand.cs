using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.Application.Commands;

public class CreateNotificationCommand : IRequest<bool>
{
    public int MessageId { get; set; }
}