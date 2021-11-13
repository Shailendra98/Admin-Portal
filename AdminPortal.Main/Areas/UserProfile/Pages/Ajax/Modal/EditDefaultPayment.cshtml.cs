using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class EditDefaultPaymentModel : PageModel
    {
        private IUserService _userService;
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public EditDefaultPaymentModel(IUserService userService, IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;
            _userService = userService;

        }
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Default Payment is required.")]
        [Display(Name = "Default Payment ")]
        public int? PaymentMethodId { get; set; }

        public SelectList PaymentMethods { get; set; }

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }


        public async Task OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(Id, cancellationToken);

            IsDone = false;
            PaymentMethodId = User.DefaultPaymentMethodId;

            PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().ToList(), "Id", "Name", User.DefaultPaymentMethodId);

        }



        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SetDefaultPaymentMethodAsync(
                    Id,
                    PaymentMethodId!.Value,
                    cancellationToken);

                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            PaymentMethods = new SelectList(Enumeration.GetAll<PaymentMethod>().ToList(), "Id", "Name", PaymentMethodId);

            return Page();
        }
    }
}
