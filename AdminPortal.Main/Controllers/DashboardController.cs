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
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;

namespace TKW.AdminPortal.Controllers
{
    
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IAppUserService _appUser;
        private readonly IDashboardQueries _dashboardQueries;
        private readonly IPickupSessionQueries _pickupSessionQueries;
        private readonly IRequestQueries _requestQueries;

        public DashboardController(IAppUserService appUser, IDashboardQueries dashboardQueries,IPickupSessionQueries pickupSessionQueries,IRequestQueries requestQueries)
        {
            _appUser = appUser;
            _dashboardQueries = dashboardQueries;
            _pickupSessionQueries = pickupSessionQueries;
            _requestQueries = requestQueries;
        }

        [HttpGet("~/api/GetVehicleAndPickupBoyCountsOfFranchise")]
        public async Task<VehicleAndpickupBoyModel> GetVehicleAndPickupBoyCountsOfFranchise(CancellationToken cancellationToken)
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
        [HttpGet("~/api/GetRequestCountsOfFranchise")]
        public async Task<RequestCountsModel> GetRequestCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            RequestCountsModel model = new RequestCountsModel
            {
                Unassigned = data.Unassigned,
                Assigned = data.Assigned,
                Handled = data.Handled,
                Rescheduled = data.Rescheduled,
                Onspot = data.Onspot,
                Cancelled = data.Cancelled
            };
            return model;

        }
       
        [HttpGet("~/api/GetpaymentMethodCountsOfFranchise")]
        public async Task<PaymentMethodModel> GetpaymentMethodCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.PaymentMethodCountOfFranchiseAsync(franchiseid.Value, cancellationToken);
            PaymentMethodModel model = new PaymentMethodModel
            {
                PaytmWallet = data.PaytmWallet,
                UPI = data.UPI,
                Cash = data.Cash,
                IMPS = data.IMPS,
                UserWallet = data.UserWallet
            };
            return model;

        }
        [HttpGet("~/api/GetpaymentMethodAmountOfFranchise")]
        public async Task<PaymentMethodModel> GetpaymentMethodAmountOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.PaymentMethodAmountOfFranchiseAsync(franchiseid.Value, cancellationToken);
            PaymentMethodModel model = new PaymentMethodModel
            {
                PaytmWallet = data.PaytmWallet,
                UPI = data.UPI,
                Cash = data.Cash,
                IMPS = data.IMPS,
                UserWallet = data.UserWallet
            };
            return model;

        }
        [HttpGet("~/api/GetPurchaseSellExpenseOfFranchise")]
        public async Task<PurchaseSellExpenseModel> GetPurchaseSellExpenseOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.PurchaseSellExpenseAmountOfFranchiseAsync(franchiseid.Value, cancellationToken);
            PurchaseSellExpenseModel model = new PurchaseSellExpenseModel
            {
                PurchaseAmount = data.PurchaseAmount,
                SellAmount = data.SellAmount,
                ExpenseAmount = data.ExpenseAmount
            };
            return model;

        }
        [HttpGet("~/api/GetAddressTypeCountsOfFranchise")]
        public async Task<AddressTypeModel> GetAddressTypeCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.AddressTypeCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            AddressTypeModel model = new AddressTypeModel
            {
                Home = data.Home,
                Shop = data.Shop,
                Office = data.Other,
                Other = data.Other,
                FoodOutlet = data.FoodOutlet,
                Institute = data.Institute,
                MallOutlete = data.MallOutlete
            };
            return model;

        }
        [HttpGet("~/api/GetCancelledRequestCountsOfFranchise")]
        public async Task<RequestSourceModel> GetCancelledRequestCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.CancelledRequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            RequestSourceModel model = new RequestSourceModel
            {
                UserApp = data.UserApp,
                UserPortal = data.UserPortal,
                AdminPortal = data.AdminPortal,
                PickupBoyApp = data.PickupBoyApp
            };
            return model;

        }
        [HttpGet("~/api/GetRescheduleRequestCountsOfFranchise")]
        public async Task<RequestSourceModel> GetRescheduleRequestCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RescheduledRequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            RequestSourceModel model = new RequestSourceModel
            {
                UserApp = data.UserApp,
                UserPortal = data.UserPortal,
                AdminPortal = data.AdminPortal,
                PickupBoyApp = data.PickupBoyApp
            };
            return model;

        }
       
        [HttpGet("~/api/GetRequestSourceCountsOfFranchise")]
        public async Task<RequestSourceModel> GetRequestSourceCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestSourceCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            RequestSourceModel model = new RequestSourceModel
            {
                UserApp = data.UserApp,
                UserPortal = data.UserPortal,
                AdminPortal = data.AdminPortal,
                PickupBoyApp = data.PickupBoyApp
            };
            return model;

        }
        [HttpGet("~/api/GetPickupSessionsOfFranchise")]
        public async Task<List<PickupSessionModel>> GetPickupSessionsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var pickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            var requestCounts = await _requestQueries.RequestCountsOfPickupSessionsAsync(pickupSessions.Select(m => m.Id), cancellationToken);
            List<PickupSessionModel> output = new List<PickupSessionModel>();
            foreach (var pickupSession in pickupSessions)
            {
                var counts = requestCounts.FirstOrDefault(m => m.PickupSessionId == pickupSession.Id);
                if (counts != null)
                {
                    var p = new PickupSessionModel
                    {
                        Id = pickupSession.Id,
                        Cash = pickupSession.Cash,
                        PickupBoys = pickupSession.PickupBoys.Where(m=>m.EndTime==null).Select(m => new PickupBoyModel
                        {
                            Id = m.Id,
                            MobileNo = m.MobileNo,
                            Name = m.Name
                        }).ToList(),
                        VehicleName = pickupSession.Vehicle.Name,
                        VehicleNumber = pickupSession.Vehicle.Number,
                        PendingRequests = counts.Pending,
                        RescheduledRequests = counts.Rescheduled,
                        CancelledRequests = counts.Cancelled,
                        HandledRequests = counts.Handled
                    };
                    output.Add(p);
                }
            }
            return output;
        }
    }
}
