namespace Messaging.Domain.AggregatesModel.MessageAggregate;

public interface IMessageRepository : IRepository<Message>
{
    Task<Message> GetAsync(int messageId);
    Task<Message> AddAsync(Message message);
    Task<Template> AddTemplateAsync(Template template);
}