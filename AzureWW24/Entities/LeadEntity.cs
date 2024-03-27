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

        public string LeadData { get; set; }

        public string First {  get; set; }
        public string Last { get; set; }

        public string Size { get; set; }

        public string Date { get; set; }

        public string ServiceType { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string FromState { get; set; }
        public string FromCity { get; set; }
        public string FromZip { get; set; }

        public string ToState { get; set; }
        public string ToCity { get; set; }
        public string ToZip { get; set; }




     

        public LeadEntity() { }

       
    }
}
