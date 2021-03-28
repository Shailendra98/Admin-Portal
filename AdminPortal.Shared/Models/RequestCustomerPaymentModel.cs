using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class RequestCustomerPaymentModel
    {
        public int? PreferredMethodId { get; set; }
        public string PreferredMethodName { get; set; } 
        public int? ActualMethodId { get; set; }
        public string ActualMethodName { get; set; }
        public int? Amount { get; set; }

        public int? CustomerAddressId { get; set; }
        public string CustomerAddressTypeName { get; set; }

        public int? SourceAppId { get; set; }
        public string SourceAppName { get; set; }
    }
}
