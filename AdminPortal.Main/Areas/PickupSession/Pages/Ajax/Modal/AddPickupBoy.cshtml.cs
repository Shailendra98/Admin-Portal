using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;
using TKW.Queries.DTOs.PickupSession;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Franchise;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Modal
{
    public class AddPickupBoyModel : PageModel
    {
        private IAppUserService _appUser;
        private readonly IEmployeeQueries _employeeQueries;
        private IPickupSessionService _pickupSessionService;

        public AddPickupBoyModel(IPickupSessionService pickupSessionService, IEmployeeQueries employeeQueries, IAppUserService appUser)
        {
            _appUser = appUser;
            _pickupSessionService = pickupSessionService;
            _employeeQueries = employeeQueries;
        }
      

        [BindProperty(SupportsGet = true)]
        [Required]
        public int id { get; set; }

        [BindProperty]
        [Display(Name = "Handlers")]
        [Required (ErrorMessage = "You have to select atleast one pickup boy")]
        public List<int> HandlerIds { get; set; }

        public IEnumerable<EmployeeModel> Handlers { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FranchiseId { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public PickupSessionModel PickupSession { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Handlers = await _employeeQueries.FreeActivePickupEmployeeOfFranchiseAsync(_appUser.Current.FranchiseId ?? FranchiseId, cancellationToken);
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var Handler = await _pickupSessionService.AddPickupBoysToPickupSessionAsync(id, HandlerIds, cancellationToken);
                if (Handler.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = Handler.Error.Message;
            }
            Handlers = await _employeeQueries.FreeActivePickupEmployeeOfFranchiseAsync(_appUser.Current.FranchiseId ?? FranchiseId, cancellationToken);
            return Page();
        }
    }
}
