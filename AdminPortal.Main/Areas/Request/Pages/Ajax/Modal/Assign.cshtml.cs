using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class AssignModel : PageModel
    {
        private readonly IRequestService _requestService;
        private readonly IAppUserService _appUser;
        private readonly IPickupSessionQueries _pickupSessionQueries;
        private readonly IRequestQueries _requestQueries;

        public AssignModel(IRequestService requestService, IAppUserService appUser, IPickupSessionQueries pickupSessionQueries, IRequestQueries requestQueries)
        {
            _requestService = requestService;
            _appUser = appUser;
            _pickupSessionQueries = pickupSessionQueries;
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public List<long> Ids { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Pickup session is required.")]
        [Display(Name = "Pickup session")]
        public int? PickupSessionId { get; set; }

        [BindProperty]
        public bool NoSMS { get; set; }

        public bool isDone { get; set; }

        public IEnumerable<PickupSessionModel> PickupSessions { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (Ids == null || Ids.Count == 0) ErrorMessage = "Please select at least one request to assign.";
            else
            {
                var franchiseIds = await _requestQueries.DistinctFranchiseIdsOfRequestsAsync(Ids, cancellationToken);
                if (franchiseIds == null || franchiseIds.Count == 0) ErrorMessage = "Requests do not belong to any Franchise.";
                else if (franchiseIds.Count >= 2) ErrorMessage = "Requests belong to two or more different Franchises.";
                else
                    PickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(franchiseIds[0], cancellationToken);
            }
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.AssignRequestsAsync(Ids, PickupSessionId.Value, SourceApp.AdminPortal, cancellationToken);
                if (result.IsSuccess)
                {
                    isDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            
            if (Ids == null || Ids.Count == 0) ErrorMessage = "Please select at least one request to assign.";
            else
            {
                var franchiseIds = await _requestQueries.DistinctFranchiseIdsOfRequestsAsync(Ids, cancellationToken);
                if (franchiseIds == null || franchiseIds.Count == 0) ErrorMessage = "Requests do not belong to any Franchise.";
                else if (franchiseIds.Count >= 2) ErrorMessage = "Requests belong to two or more different Franchises.";
                else
                    PickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(franchiseIds[0], cancellationToken);
            }
            return Page();
        }
    }
}