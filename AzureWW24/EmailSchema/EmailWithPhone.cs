﻿using AzureWW24.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWW24.EmailSchema
{
    public class EmailWithPhone
    {
        public string GetSubjectWithPhone(LeadEntity leadEntity)
        {
            string head = "A New Lead Has Arrived for " + leadEntity.Lead.Date + " with phone";
            return head;
        }

        public string GetBodyWithPhone(LeadEntity leadEntity)
        {
            string body = "Dear sales,\r\n" +
                "" + leadEntity.Lead.Name.First + " " + leadEntity.Lead.Name.Last + " has reqestest a move with following data.\r\n" +
                "Date: " + leadEntity.Lead.Date + "\r\n" +
                "Size: " + leadEntity.Lead.Size + "\r\n" +
                "Service type: " + leadEntity.Lead.ServiceType + "\r\n" +
                "Moving from: " + leadEntity.Lead.From.City + ", " + leadEntity.Lead.From.Zip + ", " + leadEntity.Lead.From.State + "\r\n" +
                "Moving to: " + leadEntity.Lead.To.City + ", " + leadEntity.Lead.To.Zip + ", " + leadEntity.Lead.To.State + "\r\n" +
                "You can contact the lead by email " + leadEntity.Lead.Contact.Email + "\r\n" +
                "Or by phone " + leadEntity.Lead.Contact.Phone + "\r\n" +
                "Kind regards,\r\n" +
                "Lead collector";
            return body;
        }
    }
}
