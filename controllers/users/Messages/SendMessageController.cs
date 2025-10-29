using Microsoft.AspNetCore.Mvc;

public class UserSendSms : ControllerBase
{
    private readonly IEnumerable<ISendMessage> _send;
    public UserSendSms(IEnumerable<ISendMessage> send)
    {
        _send = send;
    }
    [HttpGet("sendMessage")]
    public async Task<ActionResult> SendUsersMessage()
    {
        var response = _send.OfType<SendEmailMessage>().FirstOrDefault();
        string result = response.SendMessage("098765432", "the Sms is sent Successfully ");
        return Ok(result);
    }
}