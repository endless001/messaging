using Messaging.Application.Queries;
using Messaging.Application.ViewModels;

namespace Messaging.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMessageQueries _messageQueries;
    private readonly ILogger<MessageController> _logger;

    public MessageController(IMediator mediator,
        IMessageQueries messageQueries,
        ILogger<MessageController> logger)
    {
        _mediator = mediator;
        _messageQueries = messageQueries;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMessageCommand command)
    {
        var commandResult = await _mediator.Send(command);

        if (!commandResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("templates")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateTemplateAsync([FromBody] CreateTemplateCommand command)
    {
        var commandResult = await _mediator.Send(command);

        if (!commandResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("templates")]
    [ProducesResponseType(typeof(IEnumerable<TemplateVm>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTemplatesAsync()
    {
        var result = await _messageQueries.GetTemplatesAsync();
        return Ok(result);
    }

    [HttpGet("templates/{templateId}/properties")]
    [ProducesResponseType(typeof(IEnumerable<TemplatePropertyVm>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTemplatePropertiesAsync(int templateId)
    {
        var result = await _messageQueries.GetTemplatePropertiesAsync(templateId);
        return Ok(result);
    }
}