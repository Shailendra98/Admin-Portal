using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TKW.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Sell;
using System.Threading;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Buyer.Pages.Ajax
{
    public class BuyerListModel : PageModel
    {
        private readonly IBuyerQueries _buyerQueries;
        private IAppUserService _appUser;

        public BuyerListModel(IBuyerQueries buyerQueries,IAppUserService appUser)
        {
            _buyerQueries = buyerQueries;
            _appUser = appUser;
        }

        public List<BuyerWithAddressModel> Buyers { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Buyers = await _buyerQueries.BuyersOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
        }
    }
}
