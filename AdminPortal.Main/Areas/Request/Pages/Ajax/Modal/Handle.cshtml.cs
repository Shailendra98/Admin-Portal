using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Queries;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class HandleModel : PageModel
    {
        private IRequestService _requestService;
        private readonly IUserQueries _userQueries;
        private readonly IMaterialQueries _materialQueries;
        
        public HandleModel(IRequestService requestService, IUserQueries userQueries, IMaterialQueries materialQueries)
        {
            _requestService = requestService;
            _userQueries = userQueries;
            _materialQueries = materialQueries;

        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public long Id { get; set; }

        [Required]
        [BindProperty]
        public ViewModels.HandleModel HandleModel1 { get; set; }

        [Required]
        [BindProperty]
        public ViewModels.PaymentModel PaymentModel { get; set; }

        public bool IsHandled { get; set; }

        public bool IsPaymentFailed { get; set; }

        public bool IsPaid { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            PaymentModel = new ViewModels.PaymentModel
            {
                PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => !m.IsOnlineMethod && m.IsRequestPaymentSupport), "Id", "Name"),
            };
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (HandleModel1.Materials == null || HandleModel1.Materials.Count <= 0)
            {
                ModelState.AddModelError(nameof(HandleModel1.Materials), "At one material is required.");
            }
            else if (HandleModel1.Materials.Any(m => m.Quantity == null))
            {
                ModelState.AddModelError(nameof(HandleModel1.Materials), "All quantities of materials are required.");
            }
            else if (HandleModel1.Materials.Any(m => m.Rate == null))
            {
                ModelState.AddModelError(nameof(HandleModel1.Materials), "All rates of materials are required.");
            }
            if (HandleModel1.HandlerIds == null || HandleModel1.HandlerIds.Count <= 0)
            {
                ModelState.AddModelError(nameof(HandleModel1.HandlerIds), "At least one handler is required.");
            }
            if (ModelState.IsValid)
            {
                IsHandled = false;
                var result = await _requestService.HandleRequestAsync(
                     Id,
                     HandleModel1.Materials.Select(m => new RequestItemInputModel { MaterialId = m.Id.GetValueOrDefault(0), Rate = m.Rate.Value, Quantity = m.Quantity.Value }),
                     HandleModel1.HandlerIds,
                     SourceApp.AdminPortal,
                     null,
                     HandleModel1.HandleEndTime);
                if (result.IsSuccess) IsHandled = true;
                else ErrorMessage = result.Error.Message;
                if (!PaymentModel.PaymentMethodId.HasValue) IsPaid = false;
                else if (IsHandled)
                {
                    var paymentResult = await _requestService.RequestPaymentAsync(Id,PaymentModel.PaymentMethodId.Value,payerId: PaymentModel.OfflinePayerId);
                    if (paymentResult.IsSuccess)
                    {
                        IsPaid = true;
                        IsPaymentFailed = false;
                        return Page();
                    }
                    ErrorMessage = paymentResult.Error;
                    IsPaymentFailed = true;
                    return Page();
                }
            }
            if (HandleModel1?.HandlerIds != null && HandleModel1.HandlerIds.Count > 0)
                HandleModel1.Handlers = await _userQueries.UsersByIdsAsync(HandleModel1.HandlerIds, cancellationToken);
            PaymentModel.PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().Where(m => !m.IsOnlineMethod && m.IsRequestPaymentSupport), "Id", "Name", PaymentModel.PaymentMethodId);
            if (PaymentModel.OfflinePayerId.HasValue)
                PaymentModel.OfflinePayer = await _userQueries.UserByIdAsync(PaymentModel.OfflinePayerId.Value, cancellationToken);
            if (HandleModel1?.Materials != null && HandleModel1.Materials.Count > 0)
            {
                var materials = await _materialQueries.ActivePurchaseMaterialsAsync(HandleModel1.Materials.Where(m => m.Id.HasValue).Select(m => m.Id.Value), cancellationToken);
                foreach (var m in HandleModel1.Materials)
                {
                    m.Name = materials.FirstOrDefault(a => a.Id == m.Id)?.Name ?? string.Empty;
                }
            }
            return Page();
        }
    }
}