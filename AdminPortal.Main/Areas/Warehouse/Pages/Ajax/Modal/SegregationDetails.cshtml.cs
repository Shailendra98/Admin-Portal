using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal
{
    public class SegregationDetailsModel : PageModel
    {
        private IWarehouseQueries _warehouseQueries;

        public SegregationDetailsModel(IWarehouseQueries warehouseQueries)
        {
            _warehouseQueries = warehouseQueries;
        }
        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }
        public ApplicationCore.Contexts.InventoryContext.DTOs.SegregationModel Segregation { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Segregation = await _warehouseQueries.SegregationDetailsbyIdAsync(Id, cancellationToken);
        }
    }
}
