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

            var leadEntity = new LeadEntity("leadSource", Guid.NewGuid().ToString(), lead);

            var tableClient = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "Leads");
            await tableClient.CreateIfNotExistsAsync();
            await tableClient.AddEntityAsync(leadEntity);

            return new OkObjectResult("Lead saved successfully");
        }
    }
}