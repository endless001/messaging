using System.Text.Json;
using Messaging.Application.IntegrationEvents.Events;
using Messaging.Domain.Exceptions;

namespace Messaging.Application.Commands;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessagingIntegrationEventService _messagingIntegrationEventService;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMessageCommandHandler> _logger;

    public CreateMessageCommandHandler(IMessageRepository messageRepository,
        IMessagingIntegrationEventService messagingIntegrationEventService,
        IMediator mediator,
        ILogger<CreateMessageCommandHandler> logger)
    {
        _messageRepository = messageRepository;
        _messagingIntegrationEventService = messagingIntegrationEventService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {

        var template = await _messageRepository.GetTemplateAsync(request.TemplateId);
        
        if (template is null)
        {
            throw new MessagingDomainException("error template");
        }

        var message = new Message
        {
            ToUser = request.ToUser,
            TemplateId = request.TemplateId,
            ToGroup = string.Empty,
            RawData = JsonSerializer.Serialize(request.RawData, request.RawData.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            })
        };
        var entity = await _messageRepository.AddAsync(message);
        var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!result)
        {
            return result;
        }
        
        return template.Id switch
        {
            1 => await CreateNotificationAsync(entity.Id, cancellationToken),
            2 => true,
            3 => true,
            _ => result
        };
    }

    private async Task<bool> CreateNotificationAsync(int messageId,CancellationToken cancellationToken)
    {
        var command = new CreateNotificationCommand()
        {
            MessageId = messageId
        };
        return await _mediator.Send(command, cancellationToken);
    } 
}