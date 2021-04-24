using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;
using TKW.Queries.DTOs.PickupSession;
using System.ComponentModel.DataAnnotations;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Modal
{
    public class ClosePickupSessionModel : PageModel
    {
        private readonly IPickupSessionQueries _pickupSessionQueries;
        private IPickupSessionService _pickupSessionService;
        private readonly IAppUserService _appUser;

        public ClosePickupSessionModel(IAppUserService appUser, IPickupSessionQueries pickupSessionQueries,IPickupSessionService pickupSessionService)
        {
            _appUser = appUser;
            _pickupSessionQueries = pickupSessionQueries;
            _pickupSessionService = pickupSessionService;
        }

        public bool IsFranchise { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Cash")]
        public decimal Cash { get; set; }
        
        [Required]
        [BindProperty]
        [Display(Name = "Odometer Reading")]
        public int OdometerReading { get; set; }
        public bool IsDone { get; set; }

        [BindProperty]
        public string? EndOdometerPictureName { get; set; }
        public string ErrorMessage { get; set; }
        public PickupSessionModel PickupSession { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            PickupSession = await _pickupSessionQueries.PickupSessionAsync(Id, cancellationToken);
            Cash = PickupSession.Cash;
            if(PickupSession.EndOdometerReading != null)
                OdometerReading = PickupSession.EndOdometerReading.Value;
            if (PickupSession.EndOdometerPictureName != null)
                EndOdometerPictureName = PickupSession.EndOdometerPictureName;
            
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
         
            if (ModelState.IsValid)
            {
                var result = await _pickupSessionService.ClosePickupSessionAsync(Id,Cash, OdometerReading, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }

                 ErrorMessage = result.Error.Message;
            }

            return Page();
        }
    }
}
