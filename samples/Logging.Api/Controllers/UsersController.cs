using Microsoft.AspNetCore.Mvc;

namespace Logging.Api.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = nameof(GetAll))]
    public IActionResult GetAll()
    {
        _logger.LogInformation(
            "Class: {class} - Method: {method} - Trace",
            nameof(UsersController),
            nameof(GetAll));

        return Ok(DateTime.Now);
    }
}