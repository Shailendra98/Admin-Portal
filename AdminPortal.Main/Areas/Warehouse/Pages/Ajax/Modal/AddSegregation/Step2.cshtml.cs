using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.InventoryContext.Services;
using TKW.Queries.DTOs.Account;
using TKW.Queries.DTOs.Material;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Utils;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal.AddSegregation
{
    public class Step2Model : PageModel
    {
        private readonly IMaterialQueries _materialQueries;
        private readonly IUserQueries _userQueries;
        private readonly IWarehouseService _warehouseService;

        public Step2Model(IMaterialQueries materialQueries, IUserQueries userQueries, IWarehouseService warehouseService)
        {
            _materialQueries = materialQueries;
            _userQueries = userQueries;
            _warehouseService = warehouseService;
        }

        [BindProperty(SupportsGet = true)]
        public int UnsegregatedMaterialId { get; set; }
        public PurchaseStockMaterialFullModel UnsegregatedMaterial { get; set; }

        [BindProperty(SupportsGet = true)]
        public int WarehouseId { get; set; }

        [BindProperty]
        [Display(Name = "Segregated materials")]
        public List<MaterialRateQuantityInputModel> SegregatedMaterials { get; set; } = new List<MaterialRateQuantityInputModel>();

        [BindProperty]
        [Display(Name = "Wastage quantity")]
        public decimal WastageQuantity { get; set; }

        [BindProperty]
        [Display(Name = "Handlers")]
        public List<int> HandlerIds { get; set; }

        [BindProperty]
        public string? Comment { get; set; }

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public List<UserModel> Handlers { get; set; }


        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            UnsegregatedMaterial = (await _materialQueries.PurchaseStockMaterialbyIdAsync(UnsegregatedMaterialId, cancellationToken))!;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (WastageQuantity <= 0 && (SegregatedMaterials == null || SegregatedMaterials.Where(m => m.Quantity.HasValue && m.Quantity > 0).Count() <= 0))
                ErrorMessage = "Atleast one segregated material quantity or wastage quantity is required.";
            else
            {
                if (SegregatedMaterials != null && SegregatedMaterials.Any(m => m.Quantity == null))
                    ModelState.AddModelError(nameof(SegregatedMaterials), "All quantities of materials are required.");
                if (WastageQuantity < 0)
                    ModelState.AddModelError(nameof(WastageQuantity), "Wastage Quantity can't be less than zero.");

                if (ModelState.IsValid)
                {
                    List<(int id, decimal quantity)> segregatedItems = SegregatedMaterials?.Select(m => (m.Id!.Value, m.Quantity!.Value))?.ToList() ?? new List<(int id, decimal quantity)>();
                    if (WastageQuantity > 0)
                        segregatedItems.Add((0, WastageQuantity));

                    var result = await _warehouseService.AddSegregationAsync(WarehouseId, UnsegregatedMaterialId, segregatedItems, HandlerIds, IndianDateTime.Now, Comment, cancellationToken);

                    if (result.IsSuccess)
                    {
                        IsDone = true;
                        return Page();
                    }
                    else ErrorMessage = result.Error.Message;
                }
            }

            UnsegregatedMaterial = (await _materialQueries.PurchaseStockMaterialbyIdAsync(UnsegregatedMaterialId, cancellationToken))!;

            if (HandlerIds != null && HandlerIds.Count > 0)
                Handlers = await _userQueries.UsersByIdsAsync(HandlerIds, cancellationToken);

            if (SegregatedMaterials != null && SegregatedMaterials.Count > 0)
            {
                var material = await _materialQueries.ActiveSellMaterialsAsync(cancellationToken);
                foreach (var m in SegregatedMaterials)
                    m.Name = material.FirstOrDefault(a => a.Id == m.Id)?.Name ?? string.Empty;
            }
            return Page();
        }
    }
}
