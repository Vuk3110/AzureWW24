using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Entities
{
    public class LeadAuditEntity : ITableEntity
    {
        public string PartitionKey { get; set; } 
        public string RowKey { get; set; } 
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public bool IsSuccessful { get; set; } 
        public string Details { get; set; }
        public string RequestPayload { get; set; }

    }
}
