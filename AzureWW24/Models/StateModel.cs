using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.Models
{
    public class StateModel
    {
        public string StateAcronym { get; set; }
        public int FromZip { get; set; }
        public int ToZip { get; set; }
    }
}
