using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class EditBankAccountDetailsModel : PageModel
    {
        private IUserService _userService;
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public EditBankAccountDetailsModel(IUserService userService, IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;
            _userService = userService;

        }
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty]
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [BindProperty]
        [Display(Name = "Ifsc")]
        public string? Ifsc { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public async Task OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(Id, cancellationToken);

            IsDone = false;
            AccountNumber = User!.BankAccountNumber;
            Ifsc = User!.BankIFSC;

        }



        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddBankAccountAsync(
                    Id,
                    AccountNumber,
                    Ifsc,
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
