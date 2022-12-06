using System.Dynamic;
using System.Text.Json;
using EventBus.Abstractions;
using FluentEmail.Core;
using Messaging.Domain.AggregatesModel.MessageAggregate;
using Messaging.SignalrHub.IntegrationEvents.Events;
using Messaging.SignalrHub.Options;
using Microsoft.Extensions.Options;

namespace Messaging.SignalrHub.IntegrationEvents.EventHandling;

public class SendEmailIntegrationEventHandler : IIntegrationEventHandler<SendEmailIntegrationEvent>
{
    
    private readonly IMessageRepository _messageRepository;
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailOptions _options;
    public SendEmailIntegrationEventHandler(IFluentEmail fluentEmail,
        IMessageRepository messageRepository,IOptionsSnapshot<EmailOptions> options)
    {
        _fluentEmail = fluentEmail;
        _messageRepository = messageRepository;
        _options = options.Value;
    }
    public async Task Handle(SendEmailIntegrationEvent @event)
    {
        var message = await _messageRepository.GetAsync(@event.MessageId);
        
        if (message is null)
        {
            return;
        }
        
        var content = message.Template.Content;
        var model = JsonSerializer.Deserialize<ExpandoObject>(message.RawData); 
        await _fluentEmail
            .SetFrom(_options.FromEmail)
            .To(message.ToUser)
            .Subject(message.Template.Title)
            .UsingTemplate(content, model)
            .SendAsync();
    }
}