using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal.AddSegregation
{
    public class AddPurchaseMaterialModel : PageModel
    {
        private readonly IMaterialQueries _materialQueries;

        public AddPurchaseMaterialModel(IMaterialQueries materialQueries)
        {
            _materialQueries = materialQueries;
        }

        [BindProperty]
        [Display(Name = "Unsegregated material")]
        [Required(ErrorMessage = "Unsegregated material is required.")]
        public int PurchaseMaterialId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public SelectList PurchaseMaterials { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            PurchaseMaterials = new SelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), "Id", "Name");
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid) return RedirectToPage("AddSegregation", new { PurchaseMaterialId, Id });
            return Page();
        }
    }
}
