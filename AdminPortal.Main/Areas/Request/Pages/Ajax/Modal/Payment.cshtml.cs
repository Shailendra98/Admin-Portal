using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.SeedWorks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class PaymentModel : PageModel
    {
        private IRequestService _requestService;
        private IRequestQueries _requestQueries;
        private IIdentityService _identityService;
        private readonly IAppUserService _appUser;
        public PaymentModel(IRequestService requestService, IRequestQueries requestQueries, IIdentityService identityService, IAppUserService appUser)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
            _identityService = identityService;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public long Id { get; set; }

        [Required]
        [BindProperty]
        public ViewModels.PaymentModel Model { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public string? OTP { get; set; }

        public bool ShowOTPField { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var paymentDetails = await _requestQueries.SellerPaymentDetailsAsync(Id, cancellationToken);
            Model = new ViewModels.PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => m.IsActive && m.IsRequestPaymentSupport), "Id", "Name", paymentDetails.DefaultPaymentMethodId),
                PaytmWalletNumber = paymentDetails.PaytmWalletNumber ?? paymentDetails.MobileNumber,
                AccountNumber = paymentDetails.BankAccountNumber,
                IFSC = paymentDetails.BankIfsc,
                UPIAddress = paymentDetails.UPIAddress,
                NewBankAccount = string.IsNullOrEmpty(paymentDetails.BankAccountNumber) || string.IsNullOrEmpty(paymentDetails.BankIfsc),
                NewPaytmWalletAccount = string.IsNullOrEmpty(paymentDetails.PaytmWalletNumber),
                NewUPIAddress = string.IsNullOrEmpty(paymentDetails.UPIAddress),
            };
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!Model.PaymentMethodId.HasValue)
                ModelState.AddModelError(nameof(Model.PaymentMethodId), "Payment method is required.");
            if (ModelState.IsValid)
            {
                var paymentMethod = Enumeration.FromValue<PaymentMethod>(Model.PaymentMethodId!.Value);
                bool isValid = true;
                if (paymentMethod.IsOnlineMethod)
                {
                    ShowOTPField = true;

                    if (string.IsNullOrWhiteSpace(OTP))
                    {
                        var result = await _identityService.GenerateAndSendOTPAsync(_appUser.Current.MobileNo!, SourceApp.AdminPortal, cancellationToken);
                        if (result.IsFailure)
                            ErrorMessage = result.Error.Message;
                        
                        isValid = false;
                    }
                    else
                    {
                        var result = await _identityService.CheckOTPAsync(_appUser.Current.MobileNo!, OTP, SourceApp.AdminPortal);
                        if (result.IsFailure)
                        {
                            ErrorMessage = result.Error.Message;
                            isValid = false;
                        }
                        else if(!result.Value)
                        {
                            ErrorMessage = "OTP is incorrect.";
                            isValid = false;
                        }
                    }
                }
                if (isValid)
                {
                    var pickupSessionId = await _requestQueries.ActivePickupSessionIdOfRequestAsync(Id, cancellationToken);
                    var result = await _requestService.RequestPaymentAsync(
                        Id,
                        Model.PaymentMethodId!.Value,
                        payerId: Model.OfflinePayerId,
                        paytmWalletNumber: Model.PaytmWalletNumber,
                        accountNumber: Model.AccountNumber,
                        ifsc: Model.IFSC,
                        upiAddress: Model.UPIAddress,
                        pickupSessionId: pickupSessionId
                        );
                    if (result.IsSuccess)
                    {
                        IsDone = true;
                        return Page();
                    }
                    ErrorMessage = result.Error.Message;
                }
            }
            var paymentDetails = await _requestQueries.SellerPaymentDetailsAsync(Id, cancellationToken);
            Model = new ViewModels.PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => m.IsActive && m.IsRequestPaymentSupport), "Id", "Name", Model.PaymentMethodId),
                PaytmWalletNumber = Model.PaytmWalletNumber ?? paymentDetails.PaytmWalletNumber ?? paymentDetails.MobileNumber,
                AccountNumber = Model.AccountNumber ?? paymentDetails.BankAccountNumber,
                IFSC = Model.IFSC ?? paymentDetails.BankIfsc,
                UPIAddress = Model.UPIAddress ?? paymentDetails.UPIAddress,
                NewBankAccount = string.IsNullOrEmpty(paymentDetails.BankAccountNumber) || string.IsNullOrEmpty(paymentDetails.BankIfsc),
                NewPaytmWalletAccount = string.IsNullOrEmpty(paymentDetails.PaytmWalletNumber),
                NewUPIAddress = string.IsNullOrEmpty(paymentDetails.UPIAddress),
            };
            return Page();
        }
    }
}