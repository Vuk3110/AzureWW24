using Azure;
using Azure.Data.Tables;
using AzureWW24.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Entities
{
    public class LeadSentAudit : Azure.Data.Tables.ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string LeadData { get; set; }
        [IgnoreProperty]
        public Lead Lead
        {
            get => string.IsNullOrEmpty(LeadData) ? null : JsonConvert.DeserializeObject<Lead>(LeadData);
            set => LeadData = JsonConvert.SerializeObject(value);
        }
        public bool IsSuccessful { get; set; }
        public string Details { get; set; }

    }
}
