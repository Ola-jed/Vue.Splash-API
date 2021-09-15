using MimeKit;

namespace Vue.Splash_API.Services.Mail
{
    public class EmailVerificationMail: IMailable
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
            throw new System.NotImplementedException();
        }
    }
}