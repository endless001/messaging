namespace Messaging.API.Infrastructure;

public class MessagingContextSeed
{
    public async Task SeedAsync(MessagingContext context, IWebHostEnvironment env)
    {
        await using (context)
        {
            if (!context.TemplateTypes.Any())
            {
                context.TemplateTypes.AddRange(GetPredefinedTemplateType());
            }

            await context.SaveChangesAsync();
        }
    }

    private IEnumerable<TemplateType> GetPredefinedTemplateType()
    {
        return new List<TemplateType>()
        {
            TemplateType.Notification,
            TemplateType.SMS,
            TemplateType.Email
        };
    }
}