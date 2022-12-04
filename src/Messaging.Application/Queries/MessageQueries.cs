using Messaging.Application.ViewModels;

namespace Messaging.Application.Queries;

public class MessageQueries : IMessageQueries
{
    private readonly MessagingContext _context;

    public MessageQueries(MessagingContext context)
    {
        _context = context;
    }
    
    public  async Task<IEnumerable<TemplateVm>> GetTemplatesAsync()
    {
        var items = await _context.Templates.ToListAsync();
        return items.Select(x => new TemplateVm()
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    public async Task<IEnumerable<TemplatePropertyVm>> GetTemplatePropertiesAsync(int templateId)
    {
        var items = await _context.TemplateProperties.Where(t => t.TemplateId == templateId && t.AllowSearch)
            .ToListAsync();
        return items.Select(x => new TemplatePropertyVm()
        {
            Key = x.Key,
            Description = x.Description
        });
    }
}