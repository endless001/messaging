namespace Messaging.Application.Commands;

public class CreateTemplateCommand: IRequest<bool>
{
    public string Name { get; set; }
    public int Type { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<TemplatePropertyDto> TemplateProperties { get; set; }
}

public class TemplatePropertyDto
{
    public string Key { get; set; }
    public bool AllowSearch { get; set; }
    public string Description { get; set; }
}