using Messaging.Application.ViewModels;

namespace Messaging.Application.Commands;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, bool>
{
    private readonly IMessageRepository _messageRepository;

    public CreateTemplateCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<bool> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = new Template()
        {
            Name = request.Name,
            Title = request.Title,
            Content = request.Content,
            TemplateTypeId = request.Type,
            TemplateProperties = request.TemplateProperties.Select(x => new TemplateProperty
            {
                Key = x.Key,
                Description = x.Description,
                AllowSearch = x.AllowSearch
            }).ToList()
        };
        await _messageRepository.AddTemplateAsync(template);
        return await _messageRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}