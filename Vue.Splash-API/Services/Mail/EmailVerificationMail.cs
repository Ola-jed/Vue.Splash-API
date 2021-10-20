using System.Text;
using MimeKit;

namespace Vue.Splash_API.Services.Mail
{
    public class EmailVerificationMail : IMailable
    {
        private readonly string _userName;
        private readonly string _destinationMail;
        private readonly string _token;

        public EmailVerificationMail(string userName,
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
                Subject = "Vue.Splash : Email verification",
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
            htmlBuilder.Append($"Hello {_userName} <br/>");
            htmlBuilder.Append("You are receiving this email to verify your email.<br/>");
            htmlBuilder.Append("Use the following token to prove your identity.<br />");
            htmlBuilder.Append($"Here is the token : <strong>{_token}</strong><br />");
            htmlBuilder.Append(
                $"Or click on the following link <a href=\"http://localhost:8080/account/verify/{_token}?email={_destinationMail}\">Verify email</a>.<br />");
            htmlBuilder.Append("Thanks. <br />");
            htmlBuilder.Append("<a href=\"http://localhost:8080\">Vue.Splash</a>");
            return htmlBuilder.ToString();
        }

        private string BuildTextBody()
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine($"Hello {_userName}");
            textBuilder.AppendLine("You are receiving this email to verify your email.");
            textBuilder.AppendLine("Use the following token to prove your identity.");
            textBuilder.AppendLine($"Here is the token : {_token}");
            textBuilder.AppendLine("Thanks.");
            textBuilder.AppendLine("Vue.Splash");
            return textBuilder.ToString();
        }
    }
}