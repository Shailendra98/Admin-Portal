using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Purchase;
using TKW.ApplicationCore.Contexts.PurchaseContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Request.Pages.Action
{
    [Authorize]
    public class RescheduleModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IRequestQueries _requestQueries;

        public RescheduleModel(IRequestService requestService, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
        }

        [BindProperty]
        [FromRoute]
        [Required(ErrorMessage = "Request Id is required.")]
        public long? Id { get; set; }

        [BindProperty]
        [Required]
        public ViewModels.RescheduleModel rescheduleModel { get; set; }

        public RequestModel RequestModel { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long? Id, CancellationToken cancellationToken)
        {
            if (!Id.HasValue) return BadRequest();
            RequestModel = await _requestQueries.RequestAsync(Id.Value, cancellationToken);
            if (RequestModel == null) return NotFound();
            this.Id = Id;
            rescheduleModel = new ViewModels.RescheduleModel
            {
                Reasons = await _requestQueries.RequestScheduleReasonsAsync(cancellationToken),
                TimeSlots = await _requestQueries.ActiveTimeSlotsAsync(cancellationToken)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var r = await _requestService.RescheduleRequestAsync(
                    Id.Value,
                    rescheduleModel.ReasonId.Value,
                    rescheduleModel.Date.Value,
                    rescheduleModel.TimeSlotId,
                    SourceApp.AdminPortal,
                   cancellationToken);
                if (r.IsSuccess) return RedirectToPage("../Details", new { Id });
                ErrorMessage = r.Error.Message;
            }
            RequestModel = await _requestQueries.RequestAsync(Id.Value, cancellationToken);
            rescheduleModel.Reasons = await _requestQueries.RequestScheduleReasonsAsync(cancellationToken);
            rescheduleModel.TimeSlots = await _requestQueries.ActiveTimeSlotsAsync(cancellationToken);
            return Page();
        }
    }
}