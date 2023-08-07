using System;
using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable;

public class EmailVerificationMail : IMailable
{
    private readonly string _destinationMail;
    private readonly string _frontUrl;
    private readonly string _token;
    private readonly string _userName;

    public EmailVerificationMail(string userName,
        string destinationMail,
        string token,
        string frontUrl)
    {
        _userName = userName;
        _destinationMail = destinationMail;
        _token = token;
        _frontUrl = frontUrl;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Subject = "Vue.Splash : Email verification",
            To = { MailboxAddress.Parse(_destinationMail) }
        };
        
        var builder = new BodyBuilder
        {
            HtmlBody = GetHtmlBody(),
            TextBody = GetPlainTextBody()
        };
        email.Body = builder.ToMessageBody();
        return email;
    }

    public string GetHtmlBody()
    {
        return MailTemplates
            .EmailVerificationHtml
            .Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail)
            .Replace("[[_frontUrl]]", _frontUrl)
            .Replace("[[_year]]", DateTime.Now.Year.ToString());
    }

    public string GetPlainTextBody()
    {
        return MailTemplates
            .EmailVerificationText
            .Replace("[[_userName]]", _userName)
            .Replace("[[_token]]", _token)
            .Replace("[[_destinationMail]]", _destinationMail)
            .Replace("[[_frontUrl]]", _frontUrl)
            .Replace("[[_year]]", DateTime.Now.Year.ToString());
    }
}