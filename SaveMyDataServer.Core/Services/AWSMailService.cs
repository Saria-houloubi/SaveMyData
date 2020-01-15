using MailKit.Net.Smtp;
using MimeKit;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Core.Models;
using SaveMyDataServer.Core.Sercret;
using System;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.Services
{
    /// <summary>
    /// The AWS mailbox service
    /// </summary>
    public class AWSMailService : IMailService
    {
        public async Task<bool> SendEmail(EmailModel emailModel)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SecretMailInformation.TeamName, SecretMailInformation.EmailAddress));
                message.To.Add(new MailboxAddress(emailModel.UserFullName, emailModel.UserEmail));
                message.Subject = emailModel.Subject;

                //Create the body of the eamil
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailModel.ContentHTML,
                    TextBody = emailModel.Content
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(SecretMailInformation.ServerName, SecretMailInformation.Port, true);
                    client.Authenticate(SecretMailInformation.EmailAddress, SecretMailInformation.Password);

                    await client.SendAsync(message);

                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
