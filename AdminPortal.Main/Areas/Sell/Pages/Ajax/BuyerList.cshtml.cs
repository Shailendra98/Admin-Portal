using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax
{
    public class BuyerListModel : PageModel
    {
        private ISellQueries _sellQueries;
        private IAppUserService _appUser;

        public BuyerListModel(ISellQueries sellQueries, IAppUserService appUser)
        {
            _sellQueries = sellQueries;
            _appUser = appUser;
        }

        public List<BuyerModel> Buyers { get; set; }

        public bool FranchiseMode { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current.FranchiseId.HasValue;
            if (FranchiseMode) { }
               Buyers = await _sellQueries.GetAllBuyersOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken);
        }
    }
}
