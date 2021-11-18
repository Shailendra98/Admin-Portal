using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class SetDefaultModel : PageModel
    {
        private IUserService _userService;

        public SetDefaultModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public long Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SetDefaultAddressOfUserAsync(
                    Id,
                    UserId,
                    cancellationToken);

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
