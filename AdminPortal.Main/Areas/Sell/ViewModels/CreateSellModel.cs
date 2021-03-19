using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.ViewModels;
using System.ComponentModel.DataAnnotations;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;

namespace TKW.AdminPortal.Areas.Sell.ViewModels
{
    public class CreateSellModel
    {
        /// <summary>
        /// Buyer Id
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "Buyer is required.")]
        [Display(Name = "Buyer")]
        public int? BuyerId { get; set; }

        /// <summary>
        /// Warehouse Id
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "Warehouse is required.")]
        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }

        /// <summary>
        /// Handlers' Ids
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "PickupSession are required.")]
        [Display(Name = "PickupSession")]
        public int? PickupSessionId { get; set; }


     
        [BindProperty]
        [Display(Name = "Materials")]
        [Required(ErrorMessage = "At least one Material is required.")]
        public List<int> MaterialIds { get; set; }
        public MultiSelectList? Materials { get; set; }

        /// <summary>
        /// Sell Comment
        /// </summary>
        [BindProperty]
        public string? Comment { get; set; }

        public SelectList? Buyers { get; set; }

        public List<PickupSessionOptionModel>? PickupSessions { get; set; }

        public SelectList? Warehouses { get; set; }

        public class PickupSessionOptionModel
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public string Html { get; set; }

            public PickupSessionOptionModel(PickupSessionModel pickupSession)
            {
                Id = pickupSession.Id;
                Text = pickupSession.Vehicle.Name + " (" + pickupSession.Vehicle.Number + ")" + " - " + string.Join(", ", pickupSession.PickupBoys.Where(a => a.EndTime == null).Select(a => a.Name));
                Html = pickupSession.Vehicle.Name + " (" + pickupSession.Vehicle.Number + ")" + "<br/>" + string.Join(", ", pickupSession.PickupBoys.Where(a => a.EndTime == null).Select(a => a.Name));
            }
        }
    }
}
