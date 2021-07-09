using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.Request.ViewModels;
using TKW.ApplicationCore.Constants;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.DTOs.Purchase;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;

namespace TKW.AdminPortal.Areas.Request.Pages.Add
{
    public class BulkModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IUserQueries _userQueries;
        private readonly IRequestQueries _requestQueries;
        private readonly IUserService _userService;
        private readonly IRequestService _requestService;
        private readonly IWarehouseQueries _warehouseQueries;
        public BulkModel(IAppUserService appUser, IUserQueries userQueries, IRequestQueries requestQueries, IUserService userService, IRequestService requestService, IWarehouseQueries warehouseQueries)
        {
            _appUser = appUser;
            _userQueries = userQueries;
            _requestQueries = requestQueries;
            _userService = userService;
            _requestService = requestService;
            _warehouseQueries = warehouseQueries;
        }

        [BindProperty]
        [Required]
        public NewRequestModel NewRequestModel { get; set; }

        [BindProperty]
        [Required]
        public HandleModel HandleModel { get; set; }

        [BindProperty]
        [Required]
        public PaymentModel PaymentModel { get; set; }

        [BindProperty]
        [Required]
        public StoreModel StoreModel { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public List<RequestModel> PendingRequests { get; set; }

        public HandledRequestModel? LastHandledRequest { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string? mobileNo, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(mobileNo) && Regex.IsMatch(mobileNo, RegexPatterns.MobileNo)) return RedirectToPage("Index");
            NewRequestModel = new ViewModels.NewRequestModel
            {
                MobileNo = mobileNo
            };
            var user = await _userQueries.UserByMobileNumberAsync(mobileNo, cancellationToken);
            if (user != null)
            {
                NewRequestModel.IsNewUser = false;
                NewRequestModel.MobileNo = user.MobileNo;
                NewRequestModel.Name = user.Name;
                NewRequestModel.AddressId = user.DefaultAddressId;
                NewRequestModel.IsNewAddress = (!NewRequestModel.AddressId.HasValue || NewRequestModel.AddressId == 0);
                PendingRequests = await _requestQueries.PendingRequestsOfSellerMinAsync(user.Id, cancellationToken);
                LastHandledRequest = await _requestQueries.LastHandledRequestOfSellerAsync(user.Id, cancellationToken);
            }
            else
            {
                NewRequestModel.IsNewUser = true;
            }
            if (NewRequestModel.IsNewAddress) { NewRequestModel.Address = new AdminPortal.ViewModels.AddressModel { IncludeNameMobileNo = false, OnlyLocalities = _appUser.Current.FranchiseId.HasValue }; }
            
            PaymentModel = new PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => m.IsActive && m.IsRequestPaymentSupport && !m.IsOnlineMethod), "Id", "Name", PaymentMethod.Cash.Id),
            };

            var warehouses = await _warehouseQueries.AllActiveWarehousesAsync(cancellationToken);
            StoreModel = new StoreModel
            {
                Warehouses = new SelectList(warehouses, "Id", "Name")
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var user = await _userQueries.UserByMobileNumberAsync(NewRequestModel.MobileNo!, cancellationToken);
            int userId = user?.Id ?? 0;
            if (ModelState.IsValid)
            {
                var isValid = true;
                if (user == null)
                {
                    var random = new Random();
                    var r = await _userService.CreateUserAsync(
                        NewRequestModel.Name!,
                        NewRequestModel.MobileNo!,
                        UserRole.Seller,
                        NewRequestModel.Address!.AddressLine!,
                        NewRequestModel.Address.LocalityId!.Value,
                        Enumeration.FromValue<AddressType>(NewRequestModel.Address!.AddressTypeId!.Value),
                        NewRequestModel.Address.Latitude,
                        NewRequestModel.Address.Longitude,
                        null,
                        cancellationToken);
                    if (r.IsSuccess)
                    {
                        NewRequestModel.AddressId = r.Value.address.Id;
                        NewRequestModel.IsNewUser = false;
                        NewRequestModel.IsNewAddress = false;
                        userId = r.Value.user.Id;
                    }
                    else
                    {
                        ErrorMessage = r.Error.Message;
                        isValid = false;
                    }
                }
                else if (!NewRequestModel.AddressId.HasValue)
                {
                    var addressResult = await _userService.AddUserAddressAsync(
                        user.Id,
                        NewRequestModel.Address!.AddressLine!,
                        NewRequestModel.Address.LocalityId!.Value,
                        NewRequestModel.Address!.AddressTypeId!.Value,
                        NewRequestModel.Address.Name,
                        NewRequestModel.Address.MobileNo,
                        NewRequestModel.Address.Latitude,
                        NewRequestModel.Address.Longitude,
                        cancellationToken);
                    if (addressResult.IsSuccess)
                    {
                        NewRequestModel.AddressId = addressResult.Value.Id;
                        NewRequestModel.IsNewAddress = false;
                    }
                    else
                    {
                        ErrorMessage = addressResult.Error.Message;
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    var requestResult = await _requestService.AddBulkRequestAsync(
                        userId, NewRequestModel.AddressId!.Value,
                        SourceApp.AdminPortal,
                        HandleModel.HandleEndTime,
                        HandleModel.Materials.Select(m => (m.Id!.Value, m.Rate!.Value, m.Quantity!.Value)),
                        HandleModel.HandlerIds,
                        PaymentModel.PaymentMethodId!.Value,
                        StoreModel.WarehouseId!.Value,
                        null, null,
                        NewRequestModel.Comment
                        );

                    if (requestResult.IsSuccess)
                        return RedirectToPage("../Details", new { Id = requestResult.Value.Id });
                    if (requestResult.Error is RequestAlreadySubmittedError e)
                        return RedirectToPage("../Details", new { Id = e.RequestId });
                    ErrorMessage = requestResult.Error.Message;

                }
            }
            if (user != null)
            {
                PendingRequests = await _requestQueries.PendingRequestsOfSellerMinAsync(user.Id, cancellationToken);
                NewRequestModel.Name = user.Name;
                NewRequestModel.MobileNo = user.MobileNo;
                LastHandledRequest = await _requestQueries.LastHandledRequestOfSellerAsync(user.Id, cancellationToken);
            }
            if (HandleModel.HandlerIds != null && HandleModel.HandlerIds.Count > 0)
                HandleModel.Handlers = await _userQueries.UsersByIdsAsync(HandleModel.HandlerIds, cancellationToken);

            //var paymentMethods = await _paymentService.GetAllActiveRequestPaymentMethodsAsync(cancellationToken);
            //PaymentModel = new PaymentModel
            //{
            //    PaymentMethods = new SelectList(paymentMethods, "Id", "Name", PaymentModel?.PaymentMethodId),
            //};

            //if (PaymentModel.OfflinePayerId.HasValue)
            //    PaymentModel.OfflinePayer = await _accountService.GetUserByUserIdAsync(PaymentModel.OfflinePayerId.Value, cancellationToken);
            if (NewRequestModel.IsNewAddress)
            {
                if (NewRequestModel.Address != null)
                {
                    NewRequestModel.Address.IncludeNameMobileNo = false;
                    NewRequestModel.Address.OnlyLocalities = _appUser.Current.FranchiseId.HasValue;
                }
                else
                {
                    NewRequestModel.Address = new AdminPortal.ViewModels.AddressModel
                    {
                        OnlyLocalities = _appUser.Current.FranchiseId.HasValue,
                        IncludeNameMobileNo = false
                    };
                }
            }
            PaymentModel = new PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => m.IsActive && m.IsRequestPaymentSupport && !m.IsOnlineMethod), "Id", "Name", PaymentModel.PaymentMethodId),
            };

            var warehouses = await _warehouseQueries.AllActiveWarehousesAsync(cancellationToken);
            StoreModel = new StoreModel
            {
                Warehouses = new SelectList(warehouses, "Id", "Name", StoreModel.WarehouseId)
            };
            return Page();
        }
    }
}