using Azure;
using AzureWW24.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using Azure.Data.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Entities
{
    public class LeadEntity : Azure.Data.Tables.ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Koristite JSON stringove za čuvanje kompleksnih objekata
        public string LeadData { get; set; }

        // Ignorišite ovo polje prilikom čuvanja, koristi se samo u aplikaciji
        [IgnoreProperty]
        public Lead Lead
        {
            get => string.IsNullOrEmpty(LeadData) ? null : JsonConvert.DeserializeObject<Lead>(LeadData);
            set => LeadData = JsonConvert.SerializeObject(value);
        }

        public LeadEntity() { }

        public LeadEntity(string source, string leadId, Lead lead)
        {
            PartitionKey = source;
            RowKey = leadId;
            Lead = lead;
        }
    }
}
