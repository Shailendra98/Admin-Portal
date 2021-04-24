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

namespace TKW.AdminPortal.Areas.Warehouse.Pages
{
    public class IndexModel : PageModel
    {
        private IWarehouseQueries _warehouseQueries;
        private IAppUserService _appUser;
        public IndexModel(IWarehouseQueries warehouseQueries, IAppUserService appUser)
        {
            _warehouseQueries = warehouseQueries;
            _appUser = appUser;
        }

       public IEnumerable<WarehouseModel> Warehouses { get; set; }

        public bool IsFranchise { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken) 
        {
            IsFranchise = _appUser.Current.FranchiseId != null;
            if (IsFranchise)
            {
                Warehouses = await _warehouseQueries.WarehousesOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken);
                if (Warehouses.Count() == 1)
                    return RedirectToPage("Details", new { Warehouses.First().Id }); 
            }
            return Page();
        }

    }
}