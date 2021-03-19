using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using TKW.ApplicationCore.Types;

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

        public async Task OnGetAsync( CancellationToken cancellationToken)
        {
            Purchase = await _warehouseQueries.PurchaseDetailsbyIdAsync(PurchaseStoreId, cancellationToken);
        }

    }
}
