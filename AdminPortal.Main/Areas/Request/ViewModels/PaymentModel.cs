using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TKW.ApplicationCore.Constants;
using TKW.Queries.DTOs.Account;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;
using TKW.ApplicationCore.Contexts.Shared.ValueObjects;
using TKW.AdminPortal.DataAnnotations;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class PaymentModel
    {
        [Display(Name = "Payment Mode")]
        public int? PaymentMethodId { get; set; }

        [Display(Name = "Payer")]
        public int? OfflinePayerId { get; set; }

        public SelectList? PaymentMethods { get; set; }


        [Display(Name = "Paytm Wallet Number")]
        [RequiredIf(nameof(PaymentMethodId),5,ErrorMessage ="Paytm Wallet number is required.")]
        [RegularExpression(RegexPatterns.MobileNo)]
        public string? PaytmWalletNumber { get; set; }

        [Display(Name = "UPI Address")]
        [RequiredIf(nameof(PaymentMethodId),3,ErrorMessage ="UPI address is required.")]
        [RegularExpression(RegexPatterns.UPIAddress, ErrorMessage = "UPI address is invalid.")]
        public string? UPIAddress { get; set; }

        [Display(Name = "Account Number")]
        [RequiredIf(nameof(PaymentMethodId), 4,ErrorMessage ="Account number is required.")]
        [RegularExpression(RegexPatterns.AccountNumber,ErrorMessage ="Account number is invalid.")]
        public string? AccountNumber { get; set; }

        [Display(Name = "IFSC")]
        [RequiredIf(nameof(PaymentMethodId), 4)]
        [RegularExpression(RegexPatterns.Ifsc,ErrorMessage ="IFCS is invalid.")]
        public string? IFSC { get; set; }

        public bool NewPaytmWalletAccount { get; set; }
        public bool NewUPIAddress { get; set; }
        public bool NewBankAccount { get; set; }

        public UserModel? OfflinePayer { get; set; }

    }
}
