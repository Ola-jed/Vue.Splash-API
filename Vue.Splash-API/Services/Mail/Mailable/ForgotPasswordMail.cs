using System.Text;
using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable
{
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

        public MimeMessage Build()
        {
            var email = new MimeMessage()
            {
                Subject = "Vue.Splash : Forgotten password",
                To = { MailboxAddress.Parse(_destinationMail) }
            };
            var builder = new BodyBuilder()
            {
                HtmlBody = BuildHtmlBody(),
                TextBody = BuildTextBody()
            };
            email.Body = builder.ToMessageBody();
            return email;
        }

        private string BuildHtmlBody()
        {
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append($"Hello {_userName} <br/>")
                .Append("Looks like you have lost your password on Vue.Splash.<br/>")
                .Append("Use the following token to reset your password.<br />")
                .Append($"Here is the token : <strong>{_token}</strong><br />")
                .Append($"Or click on the following link <a href=\"http://localhost:8080/account/password-reset{_token}?email={_destinationMail}\">Reset password</a>.<br />")
                .Append("If it was not you who started this process, ignore this email.<br />")
                .Append("Thanks. <br />")
                .Append("<a href=\"http://localhost:8080\">Vue.Splash</a>");
            return htmlBuilder.ToString();
        }

        private string BuildTextBody()
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine($"Hello {_userName}")
                .AppendLine("Looks like you have lost your password on Vue.Splash.")
                .AppendLine("Use the following token to reset your password.")
                .AppendLine($"Here is the token : {_token}")
                .AppendLine("If it was not you who started this process, ignore this email.")
                .AppendLine("Thanks.")
                .AppendLine("Vue.Splash");
            return textBuilder.ToString();
        }
    }
}