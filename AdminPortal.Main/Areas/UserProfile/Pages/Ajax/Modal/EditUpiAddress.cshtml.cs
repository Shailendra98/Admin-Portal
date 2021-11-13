using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class EditUpiAddressModel : PageModel
    {
        private IUserService _userService;
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public EditUpiAddressModel(IUserService userService, IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;
            _userService = userService;

        }
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }
        [BindProperty]
        [Display(Name = "Upi Address")]
        public string? UpiAddress { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public async Task OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(Id, cancellationToken);

            IsDone = false;
            UpiAddress = User!.UPIAddress;
           
        }



        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddUpiAddressAsync(
                    Id,
                    UpiAddress,
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
