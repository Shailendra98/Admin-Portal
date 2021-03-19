using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.ViewModels
{
    public class SellMaterialRateQuantityInputModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Material is required.")]
        public int? Id { get; set; }

        public string? Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Quantity is required.")]
        public decimal? Quantity { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Difference quantity is required.")]
        public decimal? DifferenceQuantity { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Rate is required.")]
        public decimal? Rate { get; set; }

        public decimal? GSTPercent { get; set; }
    }
}
