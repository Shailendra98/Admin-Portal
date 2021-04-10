using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.DataAnnotations;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Constants;

namespace TKW.AdminPortal.Areas.Buyer.ViewModels
{
    public class BuyerInputModel
    {
        [Required(ErrorMessage = "Owner name is required.")]
        [Display(Name = "Owner Name")]
        public string? OwnerName { get; set; }

        [Display(Name = "Owner Mobile number")]
        [RegularExpression(RegexPatterns.MobileNo, ErrorMessage = "Mobile number is invalid.")]
        [Required(ErrorMessage = "Mobile number is required.")]
        public string? OwnerMobileNo { get; set; }


        [Required(ErrorMessage = "Firm name is required.")]
        [Display(Name = "Firm Name")]
        public string? FirmName { get; set; }

        [Required(ErrorMessage = "GSTIN name is required.")]
        public string? GSTIN { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public AddressModel? Address { get; set; }

        public bool IsActive { get; set; }

    }
}
