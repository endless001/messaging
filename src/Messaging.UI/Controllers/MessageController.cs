using Microsoft.AspNetCore.Mvc;

namespace Messaging.UI.Controllers;

public class MessageController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public MessageController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Template()
    {
        return View();
    }
}