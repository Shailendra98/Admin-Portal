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


namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class PaymentModel : PageModel
    {
        private IRequestService _requestService;
        private IRequestQueries _requestQueries;
        public PaymentModel(IRequestService requestService, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
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

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var paymentDetails = await _requestQueries.SellerPaymentDetailsAsync(Id, cancellationToken);
            Model = new ViewModels.PaymentModel
            {
                PaymentMethods = new SelectList( Enumeration.GetAll<PaymentMethod>().Where(m=>m.IsActive && m.IsRequestPaymentSupport && !m.IsOnlineMethod), "Id", "Name", paymentDetails.DefaultPaymentMethodId),
                PaytmWalletNumber = paymentDetails.PaytmWalletNumber ?? paymentDetails.MobileNumber,
                AccountNumber = paymentDetails.BankAccountNumber,
                IFSC = paymentDetails.BankIfsc,
                UPIAddress = paymentDetails.UPIAddress,
                NewBankAccount = string.IsNullOrEmpty(paymentDetails.BankAccountNumber) || string.IsNullOrEmpty(paymentDetails.BankIfsc),
                NewPaytmWalletAccount=string.IsNullOrEmpty(paymentDetails.PaytmWalletNumber),
                NewUPIAddress = string.IsNullOrEmpty(paymentDetails.UPIAddress),
            };
        }
         
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!Model.PaymentMethodId.HasValue)
                ModelState.AddModelError(nameof(Model.PaymentMethodId), "Payment method is required.");
            if (ModelState.IsValid)
            {
                var pickupSessionId = await _requestQueries.ActivePickupSessionIdOfRequestAsync(Id, cancellationToken);
                var result = await _requestService.RequestPaymentAsync(
                    Id,
                    Model.PaymentMethodId!.Value,
                    payerId: Model.OfflinePayerId,
                    paytmWalletNumber: Model.PaytmWalletNumber,
                    accountNumber: Model.AccountNumber,
                    ifsc:Model.IFSC,
                    pickupSessionId: pickupSessionId
                    );
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            var paymentDetails = await _requestQueries.SellerPaymentDetailsAsync(Id, cancellationToken);
            Model = new ViewModels.PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => m.IsActive && m.IsRequestPaymentSupport && !m.IsOnlineMethod), "Id", "Name", Model.PaymentMethodId),
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