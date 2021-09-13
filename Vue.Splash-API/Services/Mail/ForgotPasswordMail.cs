using System.Text;
using MimeKit;

namespace Vue.Splash_API.Services.Mail
{
    public class ForgotPasswordMail: IMailable
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
            htmlBuilder.Append($"Hello {_userName} <br/>");
            htmlBuilder.Append("Looks like you have lost your password on Vue.Splash.<br/>");
            htmlBuilder.Append("Use the following token to reset your password.<br />");
            htmlBuilder.Append($"Here is the token : <strong>{_token}</strong>.<br />");
            htmlBuilder.Append("If it was not you who started this process, ignore this email.<br />");
            htmlBuilder.Append("Thanks. <br />");
            htmlBuilder.Append("<a href=\"https://localhost:8080\">Vue.Splash</a>");
            return htmlBuilder.ToString();
        }

        private string BuildTextBody()
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine($"Hello {_userName}");
            textBuilder.AppendLine("Looks like you have lost your password on Vue.Splash.");
            textBuilder.AppendLine("Use the following token to reset your password.");
            textBuilder.AppendLine($"Here is the token : {_token}.");
            textBuilder.AppendLine("If it was not you who started this process, ignore this email.");
            textBuilder.AppendLine("Thanks.");
            textBuilder.AppendLine("Vue.Splash");
            return textBuilder.ToString();
        }
    }
}