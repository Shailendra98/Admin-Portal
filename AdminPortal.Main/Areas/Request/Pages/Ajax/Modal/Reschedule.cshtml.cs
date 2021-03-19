using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class RescheduleModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IRequestQueries _requestQueries;

        public RescheduleModel(IRequestService requestService, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public List<long> Ids { get; set; }

        [BindProperty]
        public ViewModels.RescheduleModel Model { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public bool isDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            isDone = false;
            Model = new ViewModels.RescheduleModel
            {
                Reasons = await _requestQueries.RequestScheduleReasonsAsync(cancellationToken),
                TimeSlots = await _requestQueries.ActiveTimeSlotsAsync(cancellationToken)
            };
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.RescheduleRequestsAsync(
                     Ids,
                     Model.ReasonId.Value,
                     Model.Date.Value,
                     Model.TimeSlotId,
                     SourceApp.AdminPortal,
                     cancellationToken);
                if (result.IsSuccess)
                {
                    isDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            isDone = false;
            Model.Reasons = await _requestQueries.RequestScheduleReasonsAsync(cancellationToken);
            Model.TimeSlots = await _requestQueries.ActiveTimeSlotsAsync(cancellationToken);
            return Page();
        }
    }
}