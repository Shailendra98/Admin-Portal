using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
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

        [BindProperty(SupportsGet = true)]
        public int HandlerId { get; set; }
        public bool IsDone { get; set; }
        public int? DefaultPayment { get; private set; }
        public string ErrorMessage { get; set; }


        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            var User = await _userQueries.UserByIdAsync(id, cancellationToken);

            IsDone = false;
            DefaultPayment = User.DefaultPaymentMethodId;
           
           
        }



        public async Task<IActionResult> OnPostAsync(int Id, int HandlerId, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SetDefaultPaymentMethodAsync(Id, HandlerId, cancellationToken);
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
