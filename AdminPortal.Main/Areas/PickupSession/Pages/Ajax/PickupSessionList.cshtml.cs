using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.PickupSession;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    [Authorize]
    public class PickupSessionListModel : PageModel
    {

        private readonly IPickupSessionQueries _pickupSessionQueries;
        private readonly IAppUserService _appUser;

        public PickupSessionListModel(IPickupSessionQueries pickupSessionQueries, IAppUserService appUser)
        {
            _pickupSessionQueries = pickupSessionQueries;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        public PickupSessionFilterModel Filter { get; set; }

        public PagedList<PickupSessionModel> PickupSessions { get; set; }

        public async Task OnGetAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseId = _appUser.Current.FranchiseId.Value;

            PickupSessions = await _pickupSessionQueries.FilteredPickupSessionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseId = _appUser.Current.FranchiseId.Value;

            PickupSessions = await _pickupSessionQueries.FilteredPickupSessionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
    }
}