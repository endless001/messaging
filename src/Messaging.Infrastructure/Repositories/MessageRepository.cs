namespace Messaging.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly MessagingContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public MessageRepository(MessagingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Message> GetAsync(int messageId)
    {
        var message = await _context
            .Messages
            .Include(x => x.Template)
            .FirstOrDefaultAsync(m => m.Id == messageId);
        return message;
    }
    

    public Task<Message> AddAsync(Message message)
    {
        return Task.FromResult(_context.Messages.Add(message).Entity);
    }

    public Task<Template> AddTemplateAsync(Template template)
    {
        return Task.FromResult(_context.Templates.Add(template).Entity);
    }
}