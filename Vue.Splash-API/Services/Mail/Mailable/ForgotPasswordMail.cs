using System.IO;
using System.Threading.Tasks;
using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable;

public class ForgotPasswordMail : IMailable
{
    private readonly string _userName;
    private readonly string _destinationMail;
    private readonly string _token;

    public ForgotPasswordMail(string userName,
        string destinationMail,
        string token)
    {
        _userName = userName;
        _destinationMail = destinationMail;
        _token = token;
    }

    public async Task<MimeMessage> Build()
    {
        var email = new MimeMessage
        {
            Subject = "Vue.Splash : Forgotten password",
            To = { MailboxAddress.Parse(_destinationMail) }
        };
        var builder = new BodyBuilder
        {
            HtmlBody = await GetHtmlBody(),
            TextBody = await GetPlainTextBody()
        };
        email.Body = builder.ToMessageBody();
        return email;
    }

    public async Task<string> GetHtmlBody()
    {
        var htmlTemplateContent = await File.ReadAllTextAsync("Services/Mail/Mailable/Template/Html/ForgotPasswordMail.html");
        return htmlTemplateContent.Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail);
    }

    public async Task<string> GetPlainTextBody()
    {
        var textTemplateContent = await File.ReadAllTextAsync("Services/Mail/Mailable/Template/Text/ForgotPasswordMail.txt");
        return textTemplateContent.Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail);
    }
}