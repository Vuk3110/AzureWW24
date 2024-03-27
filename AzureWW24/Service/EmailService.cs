using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace AzureWW24
{
    public class EmailService
    {
        private string _smtpServer;
        private int _smtpPort;
        private bool _enableSSL;
        private string _username;
        private string _password;

        public EmailService(string smtpServer, int smtpPort, bool enableSSL, string username, string password)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _enableSSL = enableSSL;
            _username = username;
            _password = password;
        }

        public void SendEmail(string fromAddress, string toAddress, string subject, string body)
        {
            var fromMailAddress = new MailAddress(fromAddress);
            var toMailAddress = new MailAddress(toAddress);

            using (var smtp = new SmtpClient
            {
                Host = _smtpServer,
                Port = _smtpPort,
                EnableSsl = _enableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_username, _password)
            })
            using (var message = new MailMessage(fromMailAddress, toMailAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}