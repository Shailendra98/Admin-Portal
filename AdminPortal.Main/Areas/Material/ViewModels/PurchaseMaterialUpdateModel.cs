using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Material.ViewModels
{
    public class PurchaseMaterialUpdateModel
    {
        [Required]
        public int? PurchaseMaterialId { get; set; }

        public string PurchaseMaterialName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Min Rate is required.")]
        public decimal? MinRate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Rate is required.")]
        public decimal? Rate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Max Rate is required.")]
        public decimal? MaxRate { get; set; }

        public bool IncludeInPickupExecutiveRateList { get; set; }
        
        public bool IncludeInSellerRateList { get; set; }

        public string? Unit { get; set; }
    }
}
