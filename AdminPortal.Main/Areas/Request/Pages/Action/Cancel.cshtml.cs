using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Request.Pages.Action
{
    [Authorize]
    public class CancelModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IRequestQueries _requestQueries;
        public CancelModel(IRequestService requestService, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
        }

        public string ErrorMessage { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Reason is required.")]
        [Display(Name = "Reason")]
        public byte? ReasonId { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Request Id is required.")]
        [FromRoute]
        public long? Id { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public RequestModel? RequestModel { get; set; }

        public List<RequestCancellationReasonModel> Reasons { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            if (!Id.HasValue) return BadRequest();
            RequestModel = await _requestQueries.RequestAsync(Id.Value, cancellationToken);
            if (RequestModel == null) return NotFound();
            Reasons = await _requestQueries.RequestCancellationReasonsAsync(cancellationToken);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var r = await _requestService.CancelRequestAsync(Id.Value, ReasonId.Value, SourceApp.AdminPortal, cancellationToken);
                if(r.IsSuccess)
                    return RedirectToPage("../Details", new { Id });
                if (r.Error is RequestAlreadyCancelledError) return RedirectToPage("../Details", new { Id });
                else ErrorMessage = r.Error.Message;
            }
            RequestModel = await _requestQueries.RequestAsync(Id.Value, cancellationToken);
            Reasons = await _requestQueries.RequestCancellationReasonsAsync(cancellationToken);
            return Page();
        }
    }
}