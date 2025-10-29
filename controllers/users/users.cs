using Microsoft.AspNetCore.Mvc;

namespace MyApp;

public class Users : ControllerBase
{
    private readonly ILogger<Users> _logger;
    public Users(ILogger<Users> logger)
    {
        _logger = logger;
    }
    [HttpGet("/users")]

    public async Task<ActionResult> getUsers()
    {
        try
        {
            var requestData = new { StartDate = DateTime.Now, EndPoint = "User Directives" };
            _logger.LogInformation("Processing {@RequestData}", requestData);
            return Ok("the data is returned");

        }
        catch (Exception ex)
        {
            _logger.LogError("somethig  went wrong please  contact system Admin for further ");
            return BadRequest("the xceptions");
        }

    }

}
