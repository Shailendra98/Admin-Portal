using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax.Modal
{
    public class SelectPincodeModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Pincode is required.")]
        public int PincodeNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid) return RedirectToPage("PincodeDetails", new { Id, PincodeNo });
            return Page();
        }
    }
}
