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
        [HttpGet("~/api/GetRequestCountsOfFranchise")]
        public async Task<RequestCountsModel> GetRequestCountsOfFranchise(CancellationToken cancellationToken)
        {
            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestCountsOfFranchiseAsync(franchiseid!.Value, cancellationToken);
            RequestCountsModel model = new RequestCountsModel
            {
                TodaysRequests = data.TotalScheduled,
                Pending = data.Pending,
                Handled = data.Handled,
                Rescheduled = data.Rescheduled,
                Onspot = data.Onspot,
                Cancelled = data.Cancelled
            };
            return model;

        }

        [HttpGet("~/api/GetActiveVehiclePickupboyCount")]
        public async Task<VehiclePickupBoyUnassignedRequestCounts> GetActiveVehiclePickupboyCount(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.VehicleAndPickupBoyAndUnassignedCountsOfFranchiseAsync(franchiseId!.Value, cancellationToken);

            VehiclePickupBoyUnassignedRequestCounts model = new VehiclePickupBoyUnassignedRequestCounts
            {
                FreeVehicle = data.FreeVehicles,
                TotalVehicle = data.TotalVehicles,
                FreePickupBoy = data.FreePickupBoys,
                TotalPickupBoy = data.TotalPickupBoys,
                UnassignedRequest = data.UnassignedRequests,
                PendingRequest = data.PendingRequests
            };
            return model;
        }

        [HttpGet("~/api/GetRequestCustomerPaymentTypeCount")]
        public async Task<List<RequestCustomerPaymentModel>> GetRequestCustomerPaymentTypeCount(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestSourceCustomerTypePaymentMethodAsync(franchiseId.Value, cancellationToken);

            if (data == null)
                return new List<RequestCustomerPaymentModel>();
            return data.Select(m => new RequestCustomerPaymentModel {
                ActualMethodId = m.ActualMethodId,
                ActualMethodName = m.ActualMethodName,
                Amount = m.Amount,
                CustomerAddressId = m.CustomerTypeId,
                CustomerAddressTypeName = m.CustomerTypeName,
                PreferredMethodId = m.PrefferedMethodId,
                PreferredMethodName = m.PrefferedMethodName,
                SourceAppId = m.SourceAppId,
                SourceAppName = m.SourceAppName
            }).ToList();

        }

        [HttpGet("~/api/GetCustomerCount")]
        public async Task<NewCustomerModel> GetCustomerCount(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.NewAndTotalCustomerCountOfFranchiseAsync(franchiseId.Value, cancellationToken);

            NewCustomerModel model = new NewCustomerModel
            {
                NewCustomers = data.NewCustomer,
                OldCustomers = data.TotalCustomer - data.NewCustomer,
                TotalCustomers = data.TotalCustomer
            };
            return model;
        }

        [HttpGet("~/api/GetCancelledAndRescheduledRequestDetails")]
        public async Task<List<CancelledAndRescheduledRequestModel>> GetCancelledAndRescheduledRequestDetails(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.CancelledAndRescheduledRequestOfFranchiseAsync(franchiseId.Value, cancellationToken);

            if (data == null)
                return new List<CancelledAndRescheduledRequestModel>();
            return data.Select(m => new CancelledAndRescheduledRequestModel {
                IsCancelled = m.IsCancelled,
                ReasonId = m.ReasonId,
                ReasonName = m.ReasonName,
                RequestId = m.RequestId,
                SourceAppId = m.SourceAppId,
                SourceAppName = m.SourceAppName,
                
            }).ToList();
        }
        [HttpGet("~/api/GetExpenseDetailsList")]
        public async Task<List<ExpenseModel>> GetExpenseDetailsList(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.ExpensesOfFranchiseAsync(franchiseId.Value, cancellationToken);

            if (data == null)
                return new List<ExpenseModel>();
            return data.Select(m => new ExpenseModel { 
                ExpenseAmount = m.TotalAmount,
                ExpenseName = m.ExpenseTypeName,
                ExpenseId = m.ExpenseTypeId
            }).ToList();

        }
        [HttpGet("~/api/GetPurchaseAndSellMaterialList")]
        public async Task<PurchaseAndSellMaterialList> GetPurchaseAndSellMaterialList(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var purchase = await _dashboardQueries.PurchaseMaterialsOfFranchiseAsync(franchiseId.Value, cancellationToken);
            var sell = await _dashboardQueries.SellMaterialsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            PurchaseAndSellMaterialList model = new PurchaseAndSellMaterialList
            {
                PurchaseList = purchase == null ? new List<PurchaseAndSellMaterialModel>() :
                purchase.Select(m => new PurchaseAndSellMaterialModel {

                    MaterialId = m.MaterialId,
                    MaterialName = m.MaterialName,
                    MaterialAmount = m.Amount,
                    MaterialQuantity = m.Quantity,
                    UnitId = m.UnitId,
                    UnitName = m.Unitname,
                }).ToList(),
                SellList = sell == null ? new List<PurchaseAndSellMaterialModel>() :
                 sell.Select(m => new PurchaseAndSellMaterialModel
                 {
                     MaterialId = m.MaterialId,
                     MaterialName = m.MaterialName,
                     MaterialAmount = m.Amount,
                     MaterialQuantity = m.Quantity,
                     UnitId = m.UnitId,
                     UnitName = m.Unitname,
                 }).ToList()
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
                        PickupBoys = pickupSession.PickupBoys.Where(m => m.EndTime == null).Select(m => new PickupBoyModel
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

        [HttpGet("~/api/RequestWeekTrendListData")]

        public async Task<List<RequestWeekTrendModel>> RequestWeekTrendListData(CancellationToken cancellationToken)
        {
            int? franchiseId = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestTrendOfFranchiseAsync(franchiseId.Value, cancellationToken);
            return new List<RequestWeekTrendModel>();
        }

    }
}
