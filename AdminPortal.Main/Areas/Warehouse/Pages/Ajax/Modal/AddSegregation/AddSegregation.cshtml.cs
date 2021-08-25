using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.InventoryContext.Services;
using TKW.Queries.DTOs.Account;
using TKW.Queries.DTOs.Material;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal.AddSegregation
{
    public class AddSegregationModel : PageModel
    {

        private readonly IUserQueries _userQueries;
        private readonly IMaterialQueries _materialQueries;
        private IWarehouseService _warehouseService;

        public AddSegregationModel(IMaterialQueries materialQueries, IWarehouseService warehouseService, IUserQueries userQueries)
        {
            _userQueries = userQueries;
            _materialQueries = materialQueries;
            _warehouseService = warehouseService;
        }
        public PurchaseStockMaterialFullModel? UnsegregatedMaterial { get; set; }

        [BindProperty]
        public int SellMaterialId { get; set; }
        public SelectList Sellmaterials { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Warehouse is required.")]
        public int? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Unsegregated material")]
        public int PurchaseMaterialId { get; set; }

        [Required(ErrorMessage = "At least one segregated material is required.")]
        [Display(Name = "Segregated material")]
        [BindProperty]
        public List<MaterialRateQuantityInputModel> SellMaterials { get; set; }

        [BindProperty]
        [Display(Name = "Handlers")]
        public List<int> HandlerIds { get; set; }

        [BindProperty]
        [Display(Name = "Handled Time")]
        [Required(ErrorMessage = "Handled time is required.")]
        public DateTime? HandleTime { get; set; }

        [BindProperty]
        public string? Comment { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public List<UserModel> Handlers { get; set; }

        public async Task OnGetAsync( CancellationToken cancellationToken)
        {
            UnsegregatedMaterial = await _materialQueries.PurchaseStockMaterialbyIdAsync(PurchaseMaterialId, cancellationToken);
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (SellMaterials == null || SellMaterials.Count <= 0)
                ModelState.AddModelError(nameof(SellMaterials), "At least one segregated material is required.");
            else if (SellMaterials.Any(m => m.Quantity == null))
                ModelState.AddModelError(nameof(SellMaterials), "All quantities of materials are required.");

            if (ModelState.IsValid)
            {
                 var result = await _warehouseService.AddSegregationAsync(Id!.Value,
                                                                         PurchaseMaterialId,
                                                                         SellMaterials!.Select(m => (m.Id.Value, m.Quantity!.Value)).ToList(),
                                                                         HandlerIds,
                                                                         HandleTime!.Value,
                                                                         Comment);

                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else ErrorMessage = result.Error.Message;
            }
           
            if (HandlerIds != null && HandlerIds.Count > 0)
                Handlers = await _userQueries.UsersByIdsAsync(HandlerIds, cancellationToken);

            if (SellMaterials != null && SellMaterials.Count > 0)
            {
                var material = await _materialQueries.PurchaseStockMaterialbyIdAsync(PurchaseMaterialId, cancellationToken);
                foreach (var m in SellMaterials)
                    m.Name = material!.SellMaterials.FirstOrDefault(a => a.Id == m.Id)?.Name ?? string.Empty;
            }
            return Page();
        }
    }
}
