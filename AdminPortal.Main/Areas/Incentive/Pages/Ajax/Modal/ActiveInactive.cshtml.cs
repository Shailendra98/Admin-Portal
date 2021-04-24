using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax.Modal
{
    public class ActiveInactiveModel : PageModel
    {
        public readonly IIncentiveService _incentiveService;


        public ActiveInactiveModel(IIncentiveService incentiveService)
        {
            _incentiveService = incentiveService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Status { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Target { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IncentiveTypeName { get; set; }

        public bool IsDone { get; set; }
        public string Errormessage { get; set; }


        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _incentiveService.ActivateOrInactivateIncentiveTargetAsync(Id, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                }
                else
                {
                    Errormessage = result.Error.Message;
                }
            }
            return Page();
        }
    }
}
