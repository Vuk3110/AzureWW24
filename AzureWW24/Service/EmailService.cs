using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace AzureWW24
{
    public class EmailService
    {
        public static async Task<bool> SendLeadEmailAsync(string apiKey, string fromEmail, string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Lead Collector");
            var to = new EmailAddress(toEmail, "Example User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}