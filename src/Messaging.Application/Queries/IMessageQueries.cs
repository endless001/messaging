using Messaging.Application.ViewModels;

namespace Messaging.Application.Queries;

public interface IMessageQueries
{
    Task<IEnumerable<TemplateVm>> GetTemplatesAsync();
    Task<IEnumerable<TemplatePropertyVm>>  GetTemplatePropertiesAsync(int templateId);
}