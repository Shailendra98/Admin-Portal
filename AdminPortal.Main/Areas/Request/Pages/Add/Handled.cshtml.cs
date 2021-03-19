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
using TKW.ApplicationCore.Contexts.AccountContext.Queries;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Request.Pages.Add
{
    public class HandledModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IUserQueries _userQueries;
        private readonly IRequestQueries _requestQueries;
        private readonly IUserService _userService;
        private readonly IRequestService _requestService;

        public HandledModel(IAppUserService appUser, IUserQueries userQueries, IRequestQueries requestQueries, IUserService userService, IRequestService requestService)
        {
            _appUser = appUser;
            _userQueries = userQueries;
            _requestQueries = requestQueries;
            _userService = userService;
            _requestService = requestService;
        }

        [BindProperty]
        [Required]
        public NewRequestModel NewRequestModel { get; set; }

        [BindProperty]
        [Required]
        public HandleModel HandleModel { get; set; }

        //[BindProperty]
        //[Required]
        //public PaymentModel PaymentModel { get; set; }

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
            //var paymentMethods = await _paymentService.GetAllActiveRequestPaymentMethodsAsync(cancellationToken);
            //PaymentModel = new PaymentModel
            //{
            //    PaymentMethods = new SelectList(paymentMethods, "Id", "Name"),
            //};

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            var user = await _userQueries.UserByMobileNumberAsync(NewRequestModel.MobileNo, cancellationToken);
            int userId = user?.Id ?? 0;
            if (ModelState.IsValid)
            {
                var isValid = true;
                if (user == null)
                {
                    var random = new Random();
                    var r = await _userService.CreateUserAsync(
                        NewRequestModel.Name,
                        NewRequestModel.MobileNo,
                        UserRole.Seller,
                        NewRequestModel.Address.AddressLine,
                        NewRequestModel.Address.LocalityId.Value,
                        Enumeration.FromValue<AddressType>(NewRequestModel.Address.AddressTypeId.Value),
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
                        NewRequestModel.Address.AddressLine,
                        NewRequestModel.Address.LocalityId.Value,
                        NewRequestModel.Address.AddressTypeId.Value,
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
                    var requestResult = await _requestService.AddHandledRequestAsync(
                          userId,
                          NewRequestModel.AddressId ?? 0,
                          SourceApp.AdminPortal,
                          null,
                          HandleModel.HandleEndTime,
                          HandleModel.Materials.Select(m => new RequestItemInputModel
                          {
                              MaterialId = m.Id.Value,
                              Quantity = m.Quantity.Value,
                              Rate = m.Rate.Value
                          }),
                          HandleModel.HandlerIds,
                          null,
                          null,
                          null,
                          NewRequestModel.Comment,
                          cancellationToken
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
            return Page();
        }
    }
}