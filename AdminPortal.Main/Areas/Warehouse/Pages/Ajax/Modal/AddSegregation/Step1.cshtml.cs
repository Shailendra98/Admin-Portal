using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.Queries.DTOs.Material;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal.AddSegregation
{
    public class Step1Model : PageModel
    {
        private readonly IMaterialQueries _materialQueries;

        public Step1Model(IMaterialQueries materialQueries)
        {
            _materialQueries = materialQueries;
        }

        [BindProperty]
        [Display(Name ="Unsegregated material")]
        [Required(ErrorMessage ="Unsegregated material is required")]
        public int? UnsegregatedMaterialId { get; set; }

        public SelectList UnsegregatedMaterials { get; set; } 

        [BindProperty(SupportsGet = true)]
        public int WarehouseId { get; set; }

        public async Task OnGetAsync(int? unsegregatedMaterialId)
        {
            UnsegregatedMaterialId = unsegregatedMaterialId;
            var materials = await _materialQueries.ActivePurchaseStockMaterialsAsync();
            UnsegregatedMaterials = new SelectList(materials, nameof(PurchaseStockMaterialModel.Id), nameof(PurchaseStockMaterialModel.Name), UnsegregatedMaterialId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) return RedirectToPage("Step2", new { UnsegregatedMaterialId, WarehouseId });
            
            var materials = await _materialQueries.ActivePurchaseStockMaterialsAsync();
            UnsegregatedMaterials = new SelectList(materials, nameof(PurchaseStockMaterialModel.Id), nameof(PurchaseStockMaterialModel.Name), UnsegregatedMaterialId);
            return Page();
        }
    }
}
