using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.DTOs.Reporting;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Pages
{
    [Authorize]
    public class DashboardOldModel : PageModel
    {
        private IAppUserService _appUser;
        private IRequestQueries _requestQueries;
        private IPickupSessionQueries _pickupSessionQueries;
        private IDashboardQueries _dashboardQueries;
        public DashboardOldModel(IAppUserService appUser, IPickupSessionQueries pickupSessionQueries, IRequestQueries requestQueries, IDashboardQueries dashboardQueries)
        {
            _requestQueries = requestQueries;
            _appUser = appUser;
            _dashboardQueries = dashboardQueries;
            _pickupSessionQueries = pickupSessionQueries;
        }

        public bool IsFranchise { get; set; }
        public IEnumerable<Queries.DTOs.PickupSession.PickupSessionModel> PickupSessions { get; set; }

        public IEnumerable<RequestCountsOfPickupSessionModel> PickupSessionRequestCounts { get; set; }

        public RequestCountOfFranchiseModel RequestCountsOfFranchise { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                RequestCountsOfFranchise = await _dashboardQueries.RequestCountsAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                PickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken);
                PickupSessionRequestCounts = await _requestQueries.RequestCountsOfPickupSessionsAsync(PickupSessions.Select(m => m.Id), cancellationToken);
            }
            return Page();
        }
    }

}