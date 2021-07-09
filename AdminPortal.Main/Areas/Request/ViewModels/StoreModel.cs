using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using TKW.Queries.DTOs.Account;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class StoreModel
    {
        [Required(ErrorMessage = "Warehouse is required.")]
        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }

        public SelectList? Warehouses { get; set; }

    }
}
