using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Inventory;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal
{
    public class DetailsModel : PageModel
    {
        private IWarehouseQueries _warehouseQueries;

        public DetailsModel(IWarehouseQueries warehouseQueries)
        {
            _warehouseQueries = warehouseQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int PurchaseStoreId { get; set; }
        public PurchaseStoreModel Purchase { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Purchase = await _warehouseQueries.PurchaseDetailsbyIdAsync(PurchaseStoreId, cancellationToken);
        }

    }
}
