using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Sell.ViewModels
{
    /// <summary>
    /// View Model for Buyer input
    /// </summary>
    public class BuyerInputModel
    {
        /// <summary>
        /// Name of the buyer
        /// </summary>
        [Required(ErrorMessage = "Buyer name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Mobile number of the buyer
        /// </summary>
        [Required(ErrorMessage = "Mobile number of buyer is required.")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"(\d{10})", ErrorMessage = "Mobile number is invalid.")]
        public string MobileNo { get; set; }

        /// <summary>
        /// Firm name of the buyer
        /// </summary>
        [Display(Name="Firm name")]
        public string FirmName { get; set; }

        /// <summary>
        /// GSTIN number
        /// </summary>
        public string GSTIN { get; set; }

        /// <summary>
        /// Whether buyer is active
        /// </summary>
        public bool IsActive { get; set; }

        public bool NameDisabled { get; set; }

        public bool MobileNoDisabled { get; set; }
    }
}
