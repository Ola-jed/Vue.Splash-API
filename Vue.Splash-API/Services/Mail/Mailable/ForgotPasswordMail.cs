using System.IO;
using System.Threading.Tasks;
using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable;

public class ForgotPasswordMail : IMailable
{
    private readonly string _userName;
    private readonly string _destinationMail;
    private readonly string _token;
    private readonly string _frontUrl;

    public ForgotPasswordMail(string userName,
        string destinationMail,
        string token,
        string frontUrl)
    {
        _userName = userName;
        _destinationMail = destinationMail;
        _token = token;
        _frontUrl = frontUrl;
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
        return await Task.Run(() => MailTemplates
            .PasswordResetHtml
            .Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail)
            .Replace("[[_frontUrl]]", _frontUrl));
    }

    public async Task<string> GetPlainTextBody()
    {
        return await Task.Run(() => MailTemplates
            .PasswordResetText
            .Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail)
            .Replace("[[_frontUrl]]", _frontUrl));
    }
}