using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MessagingContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public NotificationRepository(MessagingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<Notification> AddAsync(Notification notification)
    {
        return Task.FromResult(_context.Notifications.Add(notification).Entity);
    }
}