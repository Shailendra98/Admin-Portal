using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.MaterialSummary
{
    public class SegregatedSummaryModel : PageModel
    {
        private readonly IWarehouseQueries _warehouseQueries;
        public SegregatedSummaryModel(IWarehouseQueries warehouseQueries)
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
        public List<SegregationMaterialSummaryModel> SegregatedMaterials { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
      
            if(Filter.StartDate != null && Filter.EndDate != null)
            SegregatedMaterials = await _warehouseQueries.FilteredSegregationSummaryOfWarehouseAsync(Filter, Id);
          
        }

        public async Task OnPostAsync(CancellationToken cancellationToken)
        {
            if (Filter.StartDate != null && Filter.EndDate != null)
                SegregatedMaterials = await _warehouseQueries.FilteredSegregationSummaryOfWarehouseAsync(Filter, Id);
        }
    }
}
