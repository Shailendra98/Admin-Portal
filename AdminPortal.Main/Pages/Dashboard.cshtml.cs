using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Charts;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Contexts.ReportingContext.DTOs;
using TKW.ApplicationCore.Contexts.ReportingContext.Queries;
using TKW.ApplicationCore.Helpers;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private IAppUserService _appUser;
        private IRequestQueries _requestQueries;
        private IPickupSessionQueries _pickupSessionQueries;
        private IDashboardQueries _dashboardQueries;
        public DashboardModel(IAppUserService appUser, IPickupSessionQueries pickupSessionQueries, IRequestQueries requestQueries, IDashboardQueries dashboardQueries)
        {
            _requestQueries = requestQueries;
            _appUser = appUser;
            _dashboardQueries = dashboardQueries;
            _pickupSessionQueries = pickupSessionQueries;
        }

        public bool IsFranchise { get; set; }
        public IEnumerable<ApplicationCore.Contexts.PickupSessionContext.DTOs.PickupSessionModel> PickupSessions { get; set; }

        public IEnumerable<RequestCountsOfPickupSessionModel> PickupSessionRequestCounts { get; set; }

        public RequestCountOfFranchiseModel RequestCountsOfFranchise { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                RequestCountsOfFranchise = await _dashboardQueries.RequestCountsOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                PickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken);
                PickupSessionRequestCounts = await _requestQueries.RequestCountsOfPickupSessionsAsync(PickupSessions.Select(m => m.Id), cancellationToken);
            }
            return Page();
        }
    }

}