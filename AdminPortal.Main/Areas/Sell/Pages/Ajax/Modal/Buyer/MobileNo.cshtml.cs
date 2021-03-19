using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal.Buyer
{
    public class MobileNoModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage="Mobile number is required.")]
        [RegularExpression(@"(\d{10})",ErrorMessage ="Mobile number is invalid.")]
        [Display(Name ="Mobile number")]
        public string MobileNo { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
                return RedirectToPage("Add", new { MobileNo });
            return Page();
        }
    }
}
