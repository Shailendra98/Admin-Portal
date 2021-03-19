using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;
using TKW.ApplicationCore.Types;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Lists
{
    public class PurchaseListModel : PageModel
    {
        private readonly IWarehouseQueries _warehouseQueries;
        public PurchaseListModel(IWarehouseQueries warehouseQueries)
        {
            _warehouseQueries = warehouseQueries;

        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateRangeFilterModel Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageNo { get; set; } = 1;

        public PagedList<WarehousePurchaseListItemModel?> Purchases { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            int size = (PageSize < 5) ? 5 : (PageSize > 100) ? 100 : PageSize;
            Purchases = await _warehouseQueries.FilteredPurchasesOfWarehouseAsync(Filter, Id, PageNo, size);
        }

        public async Task OnPostAsync(CancellationToken cancellationToken)
        {
            int size = (PageSize < 5) ? 5 : (PageSize > 100) ? 100 : PageSize;
            Purchases = await _warehouseQueries.FilteredPurchasesOfWarehouseAsync(Filter, Id, PageNo, size);
        }
       
    }
}
