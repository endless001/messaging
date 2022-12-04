namespace Messaging.Domain.AggregatesModel.NotificationAggregate;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification);
}