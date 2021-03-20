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

        [HttpGet("~/api/GetRequestAddressTypeCountsOfFranchise")]
        public async Task<VehicleAndpickupBoyModel> GetRequestAddressTypeCountsOfFranchise(CancellationToken cancellationToken)
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
    }
}
