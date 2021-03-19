using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Modal
{
    public class AddCashModel : PageModel
    {
        private IPickupSessionService _pickupSessionService;

        public AddCashModel(IPickupSessionService pickupSessionService)
        {

            _pickupSessionService = pickupSessionService;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Cash")]
        public decimal Cash { get; set; }

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {

            if (ModelState.IsValid)
            {
                var addcash = _pickupSessionService.AddCashToPickupSessionAsync(Id, Cash, cancellationToken);
                if (addcash.Result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                
               // ErrorMessage = addcash.Result.Error.Message;
            }

            return Page();
        }
    }
}
