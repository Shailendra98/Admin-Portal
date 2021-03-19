using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Constants;

namespace TKW.AdminPortal.Areas.Request.Pages.Add
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        [Display(Name ="Mobile number")]
        [RegularExpression(RegexPatterns.MobileNo,ErrorMessage ="Mobile number is invalid.")]
        [Required(ErrorMessage ="Mobile number is required.")]
        public string? MobileNo { get; set; }

        [BindProperty]
        public bool IsNew { get; set; }

        public void OnGet(string? mobileNo)
        {
            this.MobileNo = mobileNo;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (IsNew)
                    return RedirectToPage("New", new { MobileNo });
                else return RedirectToPage("Handled", new { MobileNo });
            }
            return Page();
        }
    }
}