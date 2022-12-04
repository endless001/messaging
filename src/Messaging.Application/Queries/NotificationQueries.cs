using System.Text;
using Messaging.Application.ViewModels;
using Dapper;
using Npgsql;

namespace Messaging.Application.Queries;

public class NotificationQueries : INotificationQueries
{
    public async Task<PagedListVm<NotificationVm>> GetAsync(string property, string keyword, int templateId = default,
        int pageSize = 10, int pageIndex = default)
    {
        await using var connection =
            new NpgsqlConnection("Server=localhost;Port=5432;Database=messaging;User Id=postgres;Password=postgres;");
        await connection.OpenAsync();

        var where = new StringBuilder("where 1=1");
        if (!string.IsNullOrEmpty(property) && !string.IsNullOrEmpty(keyword))
        {
            where.Append(" and raw_data ->> @Property = @Keyword");
        }

        if (templateId != default)
        {
            where.Append(" and template_id = @TemplateId");
        }

        var query = string.Format(@"
             select count(*) from notifications {0};
             
             select 
                 title,content,read,created,template_name as templateName
             from notifications
              {0}
             order by created
             limit @PageSize offset @Offset", where);

        using var reader = await connection.QueryMultipleAsync(query, new
        {
            Property = property,
            Keyword = keyword,
            TemplateId = templateId,
            Offset = pageSize * pageIndex,
            PageSize = pageSize,
        });

        var totalCount = await reader.ReadFirstAsync<int>();
        var items = await reader.ReadAsync<NotificationVm>();
        
        return new PagedListVm<NotificationVm>(pageIndex, pageSize, totalCount, items);
    }
}