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
                return new BadRequestObjectResult(validationResult.Errors.Select(e => e.ErrorMessage));
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


            await tableClient.AddEntityAsync(leadEntity);

            return new OkObjectResult("Lead saved successfully");
        }
    }
}