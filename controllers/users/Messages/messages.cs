using System.Net.Http.Headers;

public interface ISendMessage
{
    string SendMessage(string phone, string message);
}
public class SendsmsMessage : ISendMessage
{
    public string SendMessage(string phone, string message)
    {
        try
        {
            return "the Sms message is sent Succesfully";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}
public class SendEmailMessage : ISendMessage
{
    public string SendMessage(string phone, string message)
    {
        try
        {
            return "the email message is sent succesfully";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}