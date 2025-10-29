using Microsoft.AspNetCore.Mvc;

namespace MyApp;

public class Users : ControllerBase
{
    [HttpGet("/users")]
    public async Task<ActionResult> getUsers()
    {
        try
        {

            return Ok("the data is returned");

        }
        catch (Exception ex)
        {
            return BadRequest("the xceptions" + ex);
        }

    }

}
