using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.PickupSession.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleQueries _vehicleQueries;
        private readonly IFranchiseQueries _franchiseQueries; 
        private IAppUserService _appUser;
        public IndexModel(IAppUserService appUserService,IFranchiseQueries franchiseQueries, IVehicleQueries vehicleQueries)
        {
            _franchiseQueries = franchiseQueries;
            _vehicleQueries = vehicleQueries;
            _appUser = appUserService;
        }
       
        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public PickupSessionFilterModel Filter { get; set; }

        public bool FranchiseMode { get; set; }
        public IEnumerable<PickupSessionModel> PickupSessions { get; set; }
        public SelectList Vehicles { get; set; }
        public SelectList Franchises { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;
            if (!FranchiseMode)
                Franchises = new SelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", Filter?.FranchiseId);
            else
            {
                var vehicleList = await _vehicleQueries.VehiclesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                Vehicles = new SelectList(vehicleList.Select(m => new { m.Id, Text = m.Name + " (" + m.Number + ")" }), "Id", "Text", Filter?.VehicleId);
            }
        }
    }
}
