using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Buyer.ViewModels
{
    public class BuyerViewModel
    {
       
        public string OwnerName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string FirmName { get; set; }
        public string? GSTIN { get; set; }
        public bool IsActive { get; set; }
        public string? AddressLine { get; set; }
        public string? LocalityName { get; set; }
        public int? Pincode { get; set; }
    }
}
