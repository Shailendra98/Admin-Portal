using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax
{
    public class RequestListModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;
        private readonly IAppUserService _appUser;

        public RequestListModel(IRequestQueries requestQueries, IAppUserService appUser)
        {
            _requestQueries = requestQueries;
            _appUser = appUser;
        }

        public PagedList<RequestListItemModel> Requests { get; set; }

        [BindProperty(SupportsGet = true)]
        public RequestFilterSortModel Filter { get; set; }


        public async Task OnGetAsync(int? pageNo, int? pageSize)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchise = new System.Collections.Generic.List<int> { _appUser.Current.FranchiseId.Value };
            Requests = await _requestQueries.FiteredAndSortedRequestsAsync(Filter, pageNo ?? 1, size);
        }
        public async Task OnPostAsync(int? pageNo, int? pageSize)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchise = new System.Collections.Generic.List<int> { _appUser.Current.FranchiseId.Value };
            Requests = await _requestQueries.FiteredAndSortedRequestsAsync(Filter, pageNo ?? 1, size);
        }
    }
}