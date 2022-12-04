using System.Dynamic;
using System.Text.Json;
using Fluid;
using Messaging.Application.IntegrationEvents.Events;
using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.Application.Commands;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, bool>
{
    private readonly IMessageRepository _messageRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IEventBus _eventBus;
    private readonly FluidParser _parser;
    private readonly ILogger<CreateNotificationCommandHandler> _logger;

    public CreateNotificationCommandHandler(IMessageRepository messageRepository,
        INotificationRepository notificationRepository,
        IEventBus eventBus,
        ILogger<CreateNotificationCommandHandler> logger)
    {
        _messageRepository = messageRepository;
        _notificationRepository = notificationRepository;
        _eventBus = eventBus;
        _parser = new FluidParser();
        _logger = logger;
    }

    public async Task<bool> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetAsync(request.MessageId);

        if (message is null)
        {
            return false;
        }

        var rawContent = message.Template.Content;
        var model = JsonSerializer.Deserialize<ExpandoObject>(message.RawData);
        var template = _parser.Parse(rawContent);
        var context = new TemplateContext(model);
        var content = await template.RenderAsync(context);

        var notification = new Notification()
        {
            MessageId = request.MessageId,
            TemplateId = message.TemplateId,
            TemplateName = message.Template.Name,
            RawData = message.RawData,
            Title = message.Template.Title,
            Content = content,
            Created = DateTime.UtcNow,
            Read = false
        };
        await _notificationRepository.AddAsync(notification);
        var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!result)
        {
            return result;
        }

        var sendNotificationIntegrationEvent =
            new SendNotificationIntegrationEvent(content, message.ToUser, message.ToGroup);
        _eventBus.Publish(sendNotificationIntegrationEvent);
        return result;
    }
}