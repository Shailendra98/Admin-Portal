using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Types;
//using X.PagedList;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax
{
    [Authorize]
    public class SellListModel : PageModel
    {
        private readonly ISellQueries _sellQueries;
        private readonly IAppUserService _appUser;

        public SellListModel(ISellQueries sellQueries, IAppUserService appUser)
        {
            _sellQueries = sellQueries;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        public SellFilterSortModel Filter { get; set; }

        public PagedList<SellListItemModel> Sells { get; set; }

        public async Task OnGetAsync(int? pageNo,int? pageSize)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchises = new List<int> { _appUser.Current.FranchiseId.Value };
            Sells = await _sellQueries.FilteredAndSortedSellsAsync(Filter, pageNo ?? 1, size);
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchises = new List<int> { _appUser.Current.FranchiseId.Value };
            Sells = await _sellQueries.FilteredAndSortedSellsAsync(Filter, pageNo ?? 1, size);
        }
    }
}