using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;
using TKW.ApplicationCore.Types;
using System.ComponentModel.DataAnnotations;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax
{
    public class SegregationModel : PageModel
    {
        private readonly IWarehouseQueries _warehouseQueries;
        public SegregationModel(IWarehouseQueries warehouseQueries)
        {
            _warehouseQueries = warehouseQueries;

        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateRangeFilterModel Filter { get; set; }
        public void OnGet()
        {

        }

    }
}
