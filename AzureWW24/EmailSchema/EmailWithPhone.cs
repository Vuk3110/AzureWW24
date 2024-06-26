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
            string subject = "A New Lead Has Arrived for " + leadEntity.Date + " with phone";
            return subject;
        }

        public string GetBodyWithPhone(LeadEntity leadEntity)
        {
            string body = "Dear sales,\r\n" +
                "" + leadEntity.First + " " + leadEntity.Last + "has requested a move with following data.\r\n" +
                "Date: " + leadEntity.Date + "\r\n" +
                "Size: " + leadEntity.Size + "\r\n" +
                "Service type: " + leadEntity.ServiceType + "\r\n" +
                "Moving from: " + leadEntity.FromCity + ", " + leadEntity.FromZip + ", " + leadEntity.FromState + "\r\n" +
                "Moving to: " + leadEntity.ToCity + ", " + leadEntity.ToZip + ", " + leadEntity.ToState + "\r\n" +
                "You can contact the lead by email " + leadEntity.Email + "\r\n" +
                "Or by phone " + leadEntity.Phone + "\r\n" +
                "Kind regards,\r\n" +
                "Lead collector";
            return body;
        }
    }
}
