using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Purchase;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class CancelModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IRequestQueries _requestQueries;

        public CancelModel(IRequestService requestService, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public List<long> Ids { get; set; }

        [BindProperty]
        [Display(Name = "Reason")]
        [Required(ErrorMessage = "Reason is required.")]
        public int? ReasonId { get; set; }

        public List<RequestCancellationReasonModel>? Reasons { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public bool isDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Reasons = await _requestQueries.RequestCancellationReasonsAsync(cancellationToken);
            isDone = false;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.CancelRequestsAsync(Ids, ReasonId.Value, SourceApp.AdminPortal, cancellationToken);
                if (result.IsSuccess || result.Error is RequestAlreadyCancelledError)
                {
                    isDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }

            Reasons = await _requestQueries.RequestCancellationReasonsAsync(cancellationToken);
            isDone = false;
            return Page();
        }
    }
}