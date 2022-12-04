using System.Text.Json;
using Messaging.Application.IntegrationEvents.Events;

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

        var command = new CreateNotificationCommand()
        {
            MessageId = entity.Id
        };
        return await _mediator.Send(command, cancellationToken);
    }
}