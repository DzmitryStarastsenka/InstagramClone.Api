using InstagramClone.EmailSender.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace InstagramClone.EmailSender.Helpers
{
    public class EmailSenderHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSenderHelper> _logger;

        public EmailSenderHelper(IConfiguration configuration, ILogger<EmailSenderHelper> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendEmailMessage(EmailModel emailModel)
        {
            var emailConfig = "EmailSettings:";

            var fromAddress = new MailAddress(
                _configuration[emailConfig + "userName"],
                _configuration[emailConfig + "senderName"]);

            var toAddress = new MailAddress(emailModel.UserName, emailModel.FirstName + " " + emailModel.LastName);

            var fromPassword = _configuration[emailConfig + "password"];

            const string subject = "New post has been published in your subscriptions!";

            var smtp = new SmtpClient
            {
                Host = _configuration[emailConfig + "host"],
                Port = int.Parse(_configuration[emailConfig + "port"]),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = GetEmaiBody(emailModel),
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };

            smtp.Send(message);

            _logger.LogInformation($"Sent email message to {emailModel.UserName}");
        }

        private static string GetEmaiBody(EmailModel emailModel)
        {
            return @$"
            <html>
            	<body>
            		<h2>Hi {emailModel.FirstName} {emailModel.LastName}!</h2>
            		<p>{emailModel.Post.UserProfile.FirstName} {emailModel.Post.UserProfile.LastName} - {emailModel.Post.UserProfile.UserName} has published a new post!</p>
            		<p>Click here to view the details: {emailModel.ShareLink}</p>
            	</body>
            </html>";
        }
    }
}
