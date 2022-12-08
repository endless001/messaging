using Autofac;
using Messaging.Domain.AggregatesModel.MessageAggregate;
using Messaging.Infrastructure.Repositories;

namespace Messaging.SignalrHub.Infrastructure.AutofacModules;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MessageRepository>()
            .As<IMessageRepository>()
            .InstancePerLifetimeScope();
    }
}