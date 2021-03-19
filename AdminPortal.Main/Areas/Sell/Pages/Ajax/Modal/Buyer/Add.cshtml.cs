using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Sell.ViewModels;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.SellContext.Queries;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal.Buyer
{
    public class AddModel : PageModel
    {
        private ISellQueries _sellQueries;
        private IAccountService _accountService;

        public AddModel(ISellQueries sellQueries, IAccountService accountService)
        {
            _sellQueries = sellQueries;
            _accountService = accountService;
        }

        [Required]
        [BindProperty]
        public BuyerInputModel InputModel { get; set; }

        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(
            string mobileNo,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(mobileNo) || !Regex.IsMatch(mobileNo, @"(\d{10})")) return RedirectToPage("MobileNo");
            InputModel = new BuyerInputModel
            {
                MobileNo = mobileNo,
                MobileNoDisabled = true,
            };
            var user = await _accountService.GetUserByMobileNoAsync(mobileNo, cancellationToken);
            if (user != null)
            {
                InputModel.Name = user.Name;
                InputModel.NameDisabled = true;
                var buyer = await _sellQueries.GetBuyerByIdAsync(user.Id);
                if (buyer!=null)
                    return Content(ModalUtils.GenerateContent("Add Buyer", "<div class='alert alert-danger'>Buyer with this mobile number is already registered.</div>", "").ToString());
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken) 
        {
            if (ModelState.IsValid)
            {
            }
            return Page();
        }

    }
}
