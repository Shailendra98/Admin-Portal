using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Sell;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;
//using TKW.Base.Services;
//using TKW.Franchise.Services;
//using TKW.Sell.DTOs.Input;
//using TKW.Sell.DTOs.Output;
//using TKW.Sell.Services;
//using TKW.Warehouse.DTOs.Output;
//using TKW.Warehouse.Services;

namespace TKW.AdminPortal.Areas.Sell.Pages
{
    public class IndexModel : PageModel
    {
        private ISellQueries _sellQueries;
        private IAppUserService _appUser;
        private IFranchiseQueries _franchiseQueries;
        private IWarehouseQueries _warehouseQueries;
        private IBuyerQueries _buyerQueries;
        public IndexModel(ISellQueries sellQueries, IBuyerQueries buyerQueries, IAppUserService appUser, IFranchiseQueries franchiseQueries, IWarehouseQueries warehouseQueries)
        {
            _sellQueries = sellQueries;
            _buyerQueries = buyerQueries;
            _appUser = appUser;
            _franchiseQueries = franchiseQueries;
            _warehouseQueries = warehouseQueries;
        }

        public bool FranchiseMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public SellFilterSortModel Filter { get; set; }

        public MultiSelectList Franchises { get; set; }
        public MultiSelectList Warehouses { get; set; }
        public MultiSelectList Buyers { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;
            if (!FranchiseMode)
            {
                Franchises = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name"/*, Filter?.Franchises*/);
            }
            else
            {
                Warehouses = new MultiSelectList(await _warehouseQueries.WarehousesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken), "Id", "Name"/*, Filter?.Warehouses*/);
                await UpdateBuyerSelectList();
            }
        }

        private async Task UpdateBuyerSelectList()
        {
            var buyers = await _buyerQueries.ActiveBuyersOfFranchiseAsync(_appUser.Current.FranchiseId!.Value);
            Buyers = new MultiSelectList(
                buyers.Select(m =>
                new
                {
                    m.Id,
                    Text = m.OwnerName + (string.IsNullOrEmpty(m.FirmName) ? "" : $" ({m.FirmName})")
                }), "Id", "Text", Filter.Buyers);

        }
    }
}