using Messaging.Application.Queries;
using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.API.Infrastructure.AutofacModules;

public class ApplicationModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MessageQueries>()
            .As<IMessageQueries>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<NotificationQueries>()
            .As<INotificationQueries>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<MessageRepository>()
            .As<IMessageRepository>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<NotificationRepository>()
            .As<INotificationRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(CreateMessageCommandHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
    }
}