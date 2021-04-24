using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TKW.Queries.DTOs.Inventory;
using TKW.Queries.Interfaces;

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
