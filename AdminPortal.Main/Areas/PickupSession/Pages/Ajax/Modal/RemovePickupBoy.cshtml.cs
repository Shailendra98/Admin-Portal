using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.PickupSessionContext.DTOs;


namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Modal
{
    public class RemovePickupBoyModel : PageModel
    {
        private IPickupSessionService _pickupSessionService;
        private IPickupSessionQueries _pickupSessionQueries;

        public RemovePickupBoyModel(IPickupSessionService pickupSessionService, IPickupSessionQueries pickupSessionQueries)
        {
            _pickupSessionQueries = pickupSessionQueries;
            _pickupSessionService = pickupSessionService;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int HandlerId { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(int Id, int HandlerId,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _pickupSessionService.RemovePickupBoysFromPickupSessionAsync(Id, new List<int> { HandlerId }, cancellationToken);
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
