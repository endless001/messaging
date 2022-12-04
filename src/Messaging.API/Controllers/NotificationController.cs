using Messaging.Application.Queries;
using Messaging.Application.ViewModels;
using Messaging.Domain.AggregatesModel.NotificationAggregate;

namespace Messaging.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly INotificationQueries _notificationQueries;
    private readonly ILogger<MessageController> _logger;

    public NotificationController(IMediator mediator,
        INotificationQueries notificationQueries, ILogger<MessageController> logger)
    {
        _mediator = mediator;
        _notificationQueries = notificationQueries;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedListVm<NotificationVm>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] int templateId = default, [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = default,
        [FromQuery] string property = "", [FromQuery] string keyword = "")
    {
        var result = await _notificationQueries.GetAsync(property, keyword, templateId, pageSize, pageIndex);
        return Ok(result);
    }
}