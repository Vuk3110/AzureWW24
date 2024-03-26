using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AzureWW24.Models
{
    public class Lead
    {
        public string Source { get; set; }
        public Name Name { get; set; }
        public Contact Contact { get; set; }
        public string Date { get; set; }
        public int Size { get; set; }
  
        public string ServiceType { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
    }
}
