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

        public SelectList Warehouses { get; set; }

        [Display(Name = ("Storer"))]
        [Required(ErrorMessage = "Storer is required.")]
        public int? StorerId { get; set; }

        public UserModel Storer { get; set; }

        [Display(Name = "Store time")]
        [Required(ErrorMessage = "Store time is required.")]
        public DateTime? StoreTime { get; set; }
    }
}
