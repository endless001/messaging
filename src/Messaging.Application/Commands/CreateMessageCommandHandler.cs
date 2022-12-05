using System.Text.Json;
using Messaging.Application.IntegrationEvents.Events;
using Messaging.Application.ViewModels;
using Messaging.Domain.Exceptions;

namespace Messaging.Application.Commands;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, bool>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessagingIntegrationEventService _messagingIntegrationEventService;
    private readonly IEventBus _eventBus;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMessageCommandHandler> _logger;

    public CreateMessageCommandHandler(IMessageRepository messageRepository,
        IMessagingIntegrationEventService messagingIntegrationEventService,
        IEventBus eventBus,
        IMediator mediator,
        ILogger<CreateMessageCommandHandler> logger)
    {
        _messageRepository = messageRepository;
        _messagingIntegrationEventService = messagingIntegrationEventService;
        _eventBus = eventBus;
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

        if (template.TemplateTypeId == TemplateType.Notification.Id)
        {
            return await CreateNotificationAsync(entity.Id, cancellationToken);
        }

        if (template.TemplateTypeId == TemplateType.Email.Id)
        {
            return await SendEmailAsync(entity.Id);
        }

        if (template.TemplateTypeId == TemplateType.SMS.Id)
        {
            return await SendSMSAsync(entity.Id);
        }

        return result;
    }

    private Task<bool> CreateNotificationAsync(int messageId, CancellationToken cancellationToken)
    {
        var command = new CreateNotificationCommand()
        {
            MessageId = messageId
        };
        return _mediator.Send(command, cancellationToken);
    }

    private Task<bool> SendEmailAsync(int messageId)
    {
        var sendEmailIntegrationEvent = new SendEmailIntegrationEvent(messageId);
        _eventBus.Publish(sendEmailIntegrationEvent);
        return Task.FromResult(true);
    }

    private Task<bool> SendSMSAsync(int messageId)
    {
        return Task.FromResult(true);
    }
}