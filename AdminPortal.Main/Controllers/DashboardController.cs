using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.ReportingContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.ReportingContext.DTOs;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using TKW.AdminPortal.Shared.Models;

namespace TKW.AdminPortal.Controllers
{
    
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IAppUserService _appUser;
        private readonly IDashboardQueries _dashboardQueries;
        public DashboardController(IAppUserService appUser, IDashboardQueries dashboardQueries)
        {
            _appUser = appUser;
            _dashboardQueries = dashboardQueries;
        }



        [HttpGet("~/api/GetRequesAddressTypeCountsOfFranchise")]
        public async Task<VehicleAndpickupBoyModel> GetRequesAddressTypeCountsOfFranchise(CancellationToken cancellationToken)
        {
            
            int? franchiseid = _appUser.Current?.FranchiseId;
            var data= await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            VehicleAndpickupBoyModel model = new VehicleAndpickupBoyModel
            {
                ActiveVehicle = data.ActiveVehicle,
                ActivePickupBoy = data.ActivePickupBoy,
                TotalPickupBoy = data.TotalPickupBoy,
                TotalVehicle = data.TotalVehicle
            };
            return model;

        }
    }
}
