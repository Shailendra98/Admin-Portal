using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.ViewModels;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class EditPendingRequestModel
    {
        public List<MaterialRateQuantityInputModel>? Materials { get; set; }
        public string? Comment { get; set; }
        
        [DisplayName("Pickup manager")]
        public int? PickupManagerId { get; set; }

        public SelectList? PickupManagers { get; set; }
        public bool? IsFranchise { get; set; }
    }
}
