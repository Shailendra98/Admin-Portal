using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.Inventory;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax
{
    public class PurchaseModel : PageModel
    {
        private IWarehouseQueries _warehouseQueries;
        private IAppUserService _appUser;

        public PurchaseModel(IWarehouseQueries warehouseService, IAppUserService appUser)
        {
            _warehouseQueries = warehouseService;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Tab { get; set; }

        public WarehouseModel? Warehouse { get; set; }

        public List<SegregatedStockModel> SegregatedMaterial { get; set; }

        public List<UnsegregatedStockModel> UnsegregatedMaterial { get; set; }
        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var segregationList = await _warehouseQueries.SegregatedStockOfWarehouseAsync(Id, cancellationToken);
            SegregatedMaterial = segregationList.Where(m => m.Quantity > 0).OrderByDescending(m => m.Quantity).ToList();
            var unsegregationList = await _warehouseQueries.UnsegregatedStockOfWarehouseAsync(Id, cancellationToken);
            UnsegregatedMaterial = unsegregationList.Where(m => m.Quantity > 0).OrderByDescending(m => m.Quantity).ToList();
            return Page();
        }
    }
}
