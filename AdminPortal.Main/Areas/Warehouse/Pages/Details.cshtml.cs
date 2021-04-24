using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Inventory;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;
//using TKW.Base.Services;
//using TKW.Warehouse.DTOs.Output;
//using TKW.Warehouse.Services;

namespace TKW.AdminPortal.Areas.Warehouse.Pages
{
    public class DetailsModel : PageModel
    {
        private IWarehouseQueries _warehouseQueries;
        private IAppUserService _appUser;

        public DetailsModel(IWarehouseQueries warehouseService, IAppUserService appUser)
        {
            _warehouseQueries = warehouseService;
            _appUser = appUser;
        }


        [BindProperty(SupportsGet = true)]
        public string? Tab { get; set; }

        public WarehouseModel Warehouse { get; set; }

        public List<SegregatedStockModel>? SegregatedMaterial { get; set; }

        public List<UnsegregatedStockModel>? UnsegregatedMaterial { get; set; }
        public async Task<IActionResult> OnGetAsync(int Id,CancellationToken cancellationToken)
        {
            Warehouse = await _warehouseQueries.WarehouseByIdAsync(Id,cancellationToken);
            if (Warehouse == null || (_appUser.Current.FranchiseId.HasValue && _appUser.Current.FranchiseId != Warehouse.FranchiseId))
                return RedirectToPage("Index");
            var segregationList = await _warehouseQueries.SegregatedStockOfWarehouseAsync(Id, cancellationToken);
            SegregatedMaterial = segregationList.Where(m => m.Quantity > 0).OrderByDescending(m=>m.Quantity).ToList();
            var unsegregationList = await _warehouseQueries.UnsegregatedStockOfWarehouseAsync(Id, cancellationToken);
            UnsegregatedMaterial = unsegregationList.Where(m => m.Quantity > 0).OrderByDescending(m => m.Quantity).ToList();
            return Page();
        }
    }
}