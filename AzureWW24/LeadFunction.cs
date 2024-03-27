using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
using Azure.Data.Tables;
using System;
using AzureWW24.Models;
using AzureWW24.Models.Validator;
using System.Linq;
using AzureWW24.Entities;
using System.Net.Mail;
using System.Net;
using AzureWW24.EmailSchema;

namespace AzureWW24
{
    public static class LeadFunction
    {
        [FunctionName("CollectLead")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Lead lead = JsonConvert.DeserializeObject<Lead>(requestBody);


            var validator = new LeadValidator();
            var validationResult = validator.Validate(lead);

            if (!validationResult.IsValid)
            {
                var errorResponse = new
                {
                    Error = "validation error",
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                };
                await CreateAndLogAuditRecord(requestBody, false, String.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),"");
                return new BadRequestObjectResult(errorResponse);
            }

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "Leads");
            await tableClient.CreateIfNotExistsAsync();

            var leadEntity = new TableEntity
            {
                PartitionKey = "Lead",
                RowKey = Guid.NewGuid().ToString(),
            };

            leadEntity["Source"] = lead.Source;
            leadEntity["FirstName"] = lead.Name.First;
            leadEntity["LastName"] = lead.Name.Last;
            leadEntity["Email"] = lead.Contact.Email;
            leadEntity["Phone"] = lead.Contact.Phone ?? "";
            leadEntity["Date"] = lead.Date;
            leadEntity["Size"] = lead.Size.ToString();
            leadEntity["ServiceType"] = lead.ServiceType;
            leadEntity["FromState"] = lead.From.State;
            leadEntity["FromCity"] = lead.From.City;
            leadEntity["FromZip"] = lead.From.Zip.ToString();
            leadEntity["ToState"] = lead.To.State;
            leadEntity["ToCity"] = lead.To.City;
            leadEntity["ToZip"] = lead.To.Zip.ToString();

            string requestPayload = System.Text.Json.JsonSerializer.Serialize(lead);


            await CreateAndLogAuditRecord(requestBody, true, "Request processed successfully", leadEntity.RowKey);

            await tableClient.AddEntityAsync(leadEntity);


            return new OkObjectResult("");
        }

       

        [FunctionName("SendEmail")]
        public static async Task SendMail([TimerTrigger("*/30  *  * * * *")] TimerInfo myTimer,  ILogger log)



        { 
            

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "Leads");


           
            var leadToDelete = tableClient.Query<LeadEntity>().FirstOrDefault();
           
            if(leadToDelete != null)
            {

            try
            {
               
                     string rowKey = leadToDelete.RowKey;

                    string smtpServer = "smtp.gmail.com";
                    int smtpPort = 587;
                    bool enableSSL = true;
                    string username = "vuktrific@gmail.com";
                    string password = "giro xwib hfub celi";
                    string message;

                    var emailService = new EmailService(smtpServer, smtpPort, enableSSL, username, password);

                    string subject = "";
                    string body = "";

                    string toAddress = leadToDelete.FromState == "MD" ? Environment.GetEnvironmentVariable("MarylandEmail")
                        : Environment.GetEnvironmentVariable("NonMarylandEmail");

                    string fromAddress = Environment.GetEnvironmentVariable("fromAddress");
                    if (!string.IsNullOrEmpty(leadToDelete.Phone))
                    {
                        EmailWithPhone emailWithPhone = new();
                        body = emailWithPhone.GetBodyWithPhone(leadToDelete);
                        subject = emailWithPhone.GetSubjectWithPhone(leadToDelete);

                    }

                    else
                    {
                        EmailWithoutPhone emailWithoutPhone = new();
                        body=emailWithoutPhone.GetBodyWithoutPhone(leadToDelete);
                        subject=emailWithoutPhone.GetSubjectWithoutPhone(leadToDelete);
                    }
                    
                  
                  

                    bool sentMail;
                 
                    try
                    {
                        
                        emailService.SendEmail(fromAddress, toAddress, subject, body);
                        message = "Email sent successfully!";
                        
                         sentMail = true;
                    }
                    catch (Exception ex)
                    {
                        message= ex.Message;
                      
                         sentMail = false;
                    }

                    await CreateLeadSentAudit(sentMail, message, rowKey);
                   
                    await tableClient.DeleteEntityAsync(leadToDelete.PartitionKey, leadToDelete.RowKey);

                    
                    log.LogInformation($"Lead {leadToDelete.RowKey} processed and deleted.");
                
            }

            catch (Exception ex)
            {
                log.LogError($"Greška pri slanju emaila: {ex.Message}");
                // Opciono: Logika za pokušaje slanja email-a
                // Možete ažurirati entitet sa novim brojem pokušaja, dodati logiku za maksimalan broj pokušaja itd.
                // Ne brišite entitet, ali možete zabeležiti pokušaj
            }
            }

           

           
            }

    private static async Task CreateAndLogAuditRecord(string requestBody, bool isSuccess, string details, string rowKey)
    {
        var auditTableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "LeadsReceivedAudit");
        await auditTableClient.CreateIfNotExistsAsync();

        var auditEntity = new LeadAuditEntity
        {
            PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            RowKey =rowKey,
            IsSuccessful = isSuccess,
            Details = details,
            RequestPayload = requestBody
        };

        await auditTableClient.AddEntityAsync(auditEntity);
    }

        private static async Task CreateLeadSentAudit(bool isSuccess, string details, string rowKey)
        {
            var sentAuditTableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "LeadSentAudit");
            await sentAuditTableClient.CreateIfNotExistsAsync();

            var sentAutid = new LeadSentAudit
            {
                PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                RowKey = rowKey,
                IsSuccessful = isSuccess,
                Details = details,
                
            };

            await sentAuditTableClient.AddEntityAsync(sentAutid);
        }



    }


}
