using Messaging.Application.ViewModels;
using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.Application.Queries;

public interface INotificationQueries
{
    Task<PagedListVm<NotificationVm>> GetAsync(string property, string keyword, int templateId = default,
        int pageSize = 10, int pageIndex = default);
}