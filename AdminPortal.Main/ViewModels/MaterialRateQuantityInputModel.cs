using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.ViewModels
{
    public class MaterialRateQuantityInputModel
    {
        [BindProperty]
        [Required(ErrorMessage ="Material is required.")]
        public int? Id { get; set; }
        
        public string? Name { get; set; }

        [BindProperty]
        public decimal? Quantity { get; set; }

        [BindProperty]
        public decimal? Rate { get; set; }

        public decimal? GSTPercent { get; set; }
        
    }
}
