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

        [HttpGet("~/api/GetActiveVehiclePickupboyCount")]
        public async Task<IdleVehiclePickupBoyModel> GetActiveVehiclePickupboyCount(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            IdleVehiclePickupBoyModel model = new IdleVehiclePickupBoyModel
            {
                ActiveVehicle = 10,
                TotalVehicle = 10,
                ActivePickupBoy = 3,
                TotalPickupBoy = 12,
                Unassigned = 5,
                TodaysRequest = 10,
            };
            return model;
        }


        [HttpGet("~/api/GetRequestCustomerPaymentTypeCount")]
        public async Task<List<RequestCustomerPaymentModel>> GetRequestCustomerPaymentTypeCount(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            List<RequestCustomerPaymentModel> data = new List<RequestCustomerPaymentModel>()
            {
                new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 1,
                     PreferredMethodName = "Paytm",
                     ActualMethodId = 2,
                     ActualMethodName = "Cash",
                     Amount = 8765,
                     CustomerAddressId = 1,
                     CustomerAddressTypeName = "Home",
                     SourceAppId = 1,
                     SourceAppName = "admin Portal"
                },
                 new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 2,
                     PreferredMethodName = "Cash",
                     ActualMethodId = 1,
                     ActualMethodName = "Paytm",
                     Amount = 8765,
                     CustomerAddressId = 2,
                     CustomerAddressTypeName = "Shop",
                     SourceAppId = 2,
                     SourceAppName = "User App"
                },
                  new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 1,
                     PreferredMethodName = "Paytm",
                     ActualMethodId = 1,
                     ActualMethodName = "Cash",
                     Amount = 8765,
                     CustomerAddressId = 3,
                     CustomerAddressTypeName = "Institute",
                     SourceAppId = 3,
                     SourceAppName = "User Portal"
                },
                   new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 1,
                     PreferredMethodName = "Paytm",
                     ActualMethodId = 1,
                     ActualMethodName = "Cash",
                     Amount = 8765,
                     CustomerAddressId = 4,
                     CustomerAddressTypeName = "FoodOutlet",
                     SourceAppId = 4,
                     SourceAppName = "PickupBoy App"
                },
                    new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 2,
                     PreferredMethodName = "Paytm",
                     ActualMethodId = 1,
                     ActualMethodName = "Cash",
                     Amount = 8765,
                     CustomerAddressId = 5,
                     CustomerAddressTypeName = "Office",
                     SourceAppId = 2,
                     SourceAppName = "User App"
                },
                     new RequestCustomerPaymentModel
                {
                     PreferredMethodId = 1,
                     PreferredMethodName = "Paytm",
                     ActualMethodId = 1,
                     ActualMethodName = "Cash",
                     Amount = 8765,
                     CustomerAddressId = 6,
                     CustomerAddressTypeName = "Other",
                     SourceAppId = 2,
                     SourceAppName = "User App"
                },
            };
            return data;
        }

        [HttpGet("~/api/GetRequestsCount")]
        public async Task<RequestCountsModel> GetRequestsCount(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            RequestCountsModel model = new RequestCountsModel
            {
                TodaysRequests = 120,
                Assigned = 58, 
                Cancelled = 12,
                Onspot = 24,
                Rescheduled = 34
            };
            return model;
        }

        [HttpGet("~/api/GetCustomerCount")]
        public async Task<NewCustomerModel> GetCustomerCount(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            NewCustomerModel model = new NewCustomerModel
            {
                NewCustomers = 45,
                OldCustomers = 12,
                TotalCustomers = 57
            };
            return model;
        }

        [HttpGet("~/api/GetCancelledAndRescheduledRequestDetails")]
        public async Task<List<CancelledAndRescheduledRequestModel>> GetCancelledAndRescheduledRequestDetails(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            List<CancelledAndRescheduledRequestModel> data = new List<CancelledAndRescheduledRequestModel>()
            {
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = true,
                SourceAppName = "Admin Portal",
                ReasonName = "Not Available",
                SourceAppId=2,
                ReasonId=1,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = true,
                SourceAppName = "Website",
                ReasonName = "Reason 2",
                SourceAppId=1,
                ReasonId=2,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = true,
                SourceAppName = "Admin Portal",
                ReasonName = "Reason 2",
                SourceAppId=2,
                ReasonId=2,
                },new CancelledAndRescheduledRequestModel
                {
                IsCancelled = false,
                SourceAppName = "Website",
                ReasonName = "Not Available",
                SourceAppId=1,
                ReasonId=1,
                },new CancelledAndRescheduledRequestModel
                {
                IsCancelled = false,
                SourceAppName = "Admin Portal",
                ReasonName = "Not Available",
                SourceAppId=2,
                ReasonId=1,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = false,
                SourceAppName = "Website",
                ReasonName = "Reason 2",
                SourceAppId=1,
                ReasonId=2,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = false,
                SourceAppName = "Admin Portal",
                ReasonName = "Reason 2",
                SourceAppId=2,
                ReasonId=2,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = false,
                SourceAppName = "Website",
                ReasonName = "Not Available",
                SourceAppId=1,
                ReasonId=1,
                },
                new CancelledAndRescheduledRequestModel
                {
                IsCancelled = true,
                SourceAppName = "Website",
                ReasonName = "Not Available",
                SourceAppId=1,
                ReasonId=1,
                },
            };
            return data;
        }

        

        [HttpGet("~/api/RequestWeekTrendListData")]

        public async Task<RequestWeekTrendList> RequestWeekTrendListData(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            RequestWeekTrendList model = new RequestWeekTrendList
            {
                CancelledRequestList = new List<RequestWeekTrendModel>
                {
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(25, 03, 2021),
                        ScheduledRequestCount = 12,
                        HandledRequestCount = 11,
                        CancelledRequestCount = 1,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(24, 03, 2021),
                        ScheduledRequestCount = 12,
                        HandledRequestCount = 10,
                        CancelledRequestCount = 2,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(23, 03, 2021),
                        ScheduledRequestCount = 12,
                        HandledRequestCount = 8,
                        CancelledRequestCount = 4,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(22, 03, 2021),
                        ScheduledRequestCount = 20,
                        HandledRequestCount = 15,
                        CancelledRequestCount = 5,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(21, 03, 2021),
                        ScheduledRequestCount = 15,
                        HandledRequestCount = 10,
                        CancelledRequestCount = 5,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(20, 03, 2021),
                        ScheduledRequestCount = 14,
                        HandledRequestCount = 6,
                        CancelledRequestCount = 8,
                    },
                    new RequestWeekTrendModel
                    {
                        Date = new DateTime(19, 03, 2021),
                        ScheduledRequestCount = 12,
                        HandledRequestCount = 10,
                        CancelledRequestCount = 2,
                    }


                }


            };
            return model;
        }


        [HttpGet("~/api/GetExpenseDetailsList")]
        public async Task<ExpenseDetailsList> GetExpenseDetailsList(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            //ExpenseModel model = new ExpenseModel
            //{
            //   ExpenseName = "Newspaper",
            //   ExpenseAmount = 213,
            //};
            //return model;
            ExpenseDetailsList model = new ExpenseDetailsList
            {
                ExpensesList = new List<ExpenseModel>
                {
                    new ExpenseModel
                    {
                        ExpenseName = "Stationary",
                        ExpenseAmount = 340,
                    },
                    new ExpenseModel
                    {
                        ExpenseName = "Snacks",
                        ExpenseAmount = 120,
                    },
                    new ExpenseModel
                    {
                        ExpenseName = "Oil",
                        ExpenseAmount = 200,
                    },
                },

            };
            return model;
            
        }

        [HttpGet("~/api/GetPurchaseAndSellMaterialList")]
        public async Task<PurchaseAndSellMaterialList> GetPurchaseAndSellMaterialList(CancellationToken cancellationToken)
        {
            //int? franchiseId = _appUser.Current?.FranchiseId;
            //var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseId.Value, cancellationToken);

            PurchaseAndSellMaterialList model = new PurchaseAndSellMaterialList
            {
                PurchaseList = new List<PurchaseAndSellMaterialModel>
                {
                    new PurchaseAndSellMaterialModel
                    {
                        MaterialName = "Paper",
                        MaterialAmount = 32342,
                        MaterialQuantity = 432,
                        UnitName = "Unit 1"
                    },

                    new PurchaseAndSellMaterialModel
                    {
                        MaterialName = "Metal",
                        MaterialAmount = 3242,
                        MaterialQuantity = 4325,
                        UnitName = "Unit 2"
                    }
                },

                SellList = new List<PurchaseAndSellMaterialModel>
                {
                    new PurchaseAndSellMaterialModel
                    {
                        MaterialName = "Iron",
                        MaterialAmount = 32342,
                        MaterialQuantity = 432,
                        UnitName = "Unit 1"
                    },

                    new PurchaseAndSellMaterialModel
                    {
                        MaterialName = "Others",
                        MaterialAmount = 3242,
                        MaterialQuantity = 4325,
                        UnitName = "Unit 2"
                    }
                }

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
        //[HttpGet("~/api/GetVehicleAndPickupBoyCountsOfFranchise")]
        //public async Task<ActiveVehiclePickupBoyModel> GetVehicleAndPickupBoyCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.VehicleAndPickupBoyCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    ActiveVehiclePickupBoyModel model = new ActiveVehiclePickupBoyModel
        //    {
        //        ActiveVehicle = data.ActiveVehicle,
        //        ActivePickupBoy = data.ActivePickupBoy,
        //        TotalPickupBoy = data.TotalPickupBoy,
        //        TotalVehicle = data.TotalVehicle
        //    };
        //    return model;

        //}
        [HttpGet("~/api/GetRequestCountsOfFranchise")]
        public async Task<RequestCountsModel> GetRequestCountsOfFranchise(CancellationToken cancellationToken)
        {

            int? franchiseid = _appUser.Current?.FranchiseId;
            var data = await _dashboardQueries.RequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
            RequestCountsModel model = new RequestCountsModel
            {
                Assigned = data.Assigned,
                Handled = data.Handled,
                Rescheduled = data.Rescheduled,
                Onspot = data.Onspot,
                Cancelled = data.Cancelled
            };
            return model;

        }

        //[HttpGet("~/api/GetpaymentMethodCountsOfFranchise")]
        //public async Task<PaymentMethodModel> GetpaymentMethodCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.PaymentMethodCountOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    PaymentMethodModel model = new PaymentMethodModel
        //    {
        //        PaytmWallet = data.PaytmWallet,
        //        UPI = data.UPI,
        //        Cash = data.Cash,
        //        IMPS = data.IMPS,
        //        UserWallet = data.UserWallet
        //    };
        //    return model;

        //}
        //[HttpGet("~/api/GetpaymentMethodAmountOfFranchise")]
        //public async Task<PaymentMethodModel> GetpaymentMethodAmountOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.PaymentMethodAmountOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    PaymentMethodModel model = new PaymentMethodModel
        //    {
        //        PaytmWallet = data.PaytmWallet,
        //        UPI = data.UPI,
        //        Cash = data.Cash,
        //        IMPS = data.IMPS,
        //        UserWallet = data.UserWallet
        //    };
        //    return model;

        //}
        //[HttpGet("~/api/GetPurchaseSellExpenseOfFranchise")]
        //public async Task<PurchaseSellExpenseModel> GetPurchaseSellExpenseOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.PurchaseSellExpenseAmountOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    PurchaseSellExpenseModel model = new PurchaseSellExpenseModel
        //    {
        //        PurchaseAmount = data.PurchaseAmount,
        //        SellAmount = data.SellAmount,
        //        ExpenseAmount = data.ExpenseAmount
        //    };
        //    return model;

        //}
        //[HttpGet("~/api/GetAddressTypeCountsOfFranchise")]
        //public async Task<AddressTypeModel> GetAddressTypeCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.AddressTypeCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    AddressTypeModel model = new AddressTypeModel
        //    {
        //        Home = data.Home,
        //        Shop = data.Shop,
        //        Office = data.Other,
        //        Other = data.Other,
        //        FoodOutlet = data.FoodOutlet,
        //        Institute = data.Institute,
        //        MallOutlete = data.MallOutlete
        //    };
        //    return model;

        //}
        //[HttpGet("~/api/GetCancelledRequestCountsOfFranchise")]
        //public async Task<RequestSourceModel> GetCancelledRequestCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.CancelledRequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    RequestSourceModel model = new RequestSourceModel
        //    {
        //        UserApp = data.UserApp,
        //        UserPortal = data.UserPortal,
        //        AdminPortal = data.AdminPortal,
        //        PickupBoyApp = data.PickupBoyApp
        //    };
        //    return model;

        //}
        //[HttpGet("~/api/GetRescheduleRequestCountsOfFranchise")]
        //public async Task<RequestSourceModel> GetRescheduleRequestCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.RescheduledRequestCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    RequestSourceModel model = new RequestSourceModel
        //    {
        //        UserApp = data.UserApp,
        //        UserPortal = data.UserPortal,
        //        AdminPortal = data.AdminPortal,
        //        PickupBoyApp = data.PickupBoyApp
        //    };
        //    return model;

        //}

        //[HttpGet("~/api/GetRequestSourceCountsOfFranchise")]
        //public async Task<RequestSourceModel> GetRequestSourceCountsOfFranchise(CancellationToken cancellationToken)
        //{

        //    int? franchiseid = _appUser.Current?.FranchiseId;
        //    var data = await _dashboardQueries.RequestSourceCountsOfFranchiseAsync(franchiseid.Value, cancellationToken);
        //    RequestSourceModel model = new RequestSourceModel
        //    {
        //        UserApp = data.UserApp,
        //        UserPortal = data.UserPortal,
        //        AdminPortal = data.AdminPortal,
        //        PickupBoyApp = data.PickupBoyApp
        //    };
        //    return model;

        //}

    }
}
