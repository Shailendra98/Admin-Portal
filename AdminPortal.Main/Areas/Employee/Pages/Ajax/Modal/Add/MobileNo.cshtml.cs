using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Constants;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal.Add
{
    public class MobileNoModel : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage ="Mobile number is required.")]
        [RegularExpression(RegexPatterns.MobileNo, ErrorMessage = "Mobile number is invalid.")]
        [Display(Name ="Mobile number")]
        public string MobileNo { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid) return RedirectToPage("Add", new { MobileNo });
            return Page();
        }
    }
}
