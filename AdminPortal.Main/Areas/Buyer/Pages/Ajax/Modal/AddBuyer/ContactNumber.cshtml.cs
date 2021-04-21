using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Constants;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;
using TKW.ApplicationCore.Contexts.AccountContext.Queries;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;

namespace TKW.AdminPortal.Areas.Buyer.Pages.Ajax.Modal.AddBuyer
{
    public class ContactNumberModel : PageModel
    {
        private readonly IBuyerQueries _buyerQueries;

        private readonly IUserQueries _userQueries;

        public ContactNumberModel(IBuyerQueries buyerQueries,IUserQueries userQueries)
        {
            _buyerQueries = buyerQueries;
            _userQueries = userQueries;
        }

        [BindProperty]
        [Display(Name = "Mobile number")]
        [RegularExpression(RegexPatterns.MobileNo, ErrorMessage = "Mobile number is invalid.")]
        [Required(ErrorMessage = "Mobile number is required.")]
        public string? OwnerMobileNo { get; set; }

        public BuyerWithAddressModel? Buyer { get; set; }

        public UserModel? User { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid) {
                User = await _userQueries.UserByMobileNumberAsync(OwnerMobileNo, cancellationToken);
                if(User != null)
                {
                    Buyer = await _buyerQueries.BuyerByOwnerIdAsync(User.Id, cancellationToken);
                    if (Buyer != null)
                    {
                        return RedirectToPage("../Edit", new { BuyerId = Buyer.Id });
                    }
                }
               return RedirectToPage("AddDetails", new { OwnerMobileNo });
            }
            return Page();
        }
    }
}
