using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.MaterialSummary
{
    public class SellSummaryModel : PageModel
    {
        private readonly IWarehouseQueries _warehouseQueries;
        public SellSummaryModel(IWarehouseQueries warehouseQueries)
        {
            _warehouseQueries = warehouseQueries;

        }
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateRangeFilterModel Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public int PageNo { get; set; } = 1;

        public  List<SellMaterialSummaryModel> SellMaterials { get; set; }
        public async Task OnGetAsync( CancellationToken cancellationToken)
        {
            if (Filter.StartDate != null && Filter.EndDate != null)
                SellMaterials = await _warehouseQueries.FilteredSellSummaryOfWarehouseAsync(Filter, Id);
        }

        public async Task OnPostAsync(CancellationToken cancellationToken)
        {
            if (Filter.StartDate != null && Filter.EndDate != null)
                SellMaterials = await _warehouseQueries.FilteredSellSummaryOfWarehouseAsync(Filter, Id);
        }
    }
}
