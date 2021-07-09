using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Constants;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages.Add
{
    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUser;
        
        public IndexModel(IAppUserService appUser)
        {
            _appUser = appUser;
        }


        [BindProperty]
        [Display(Name ="Mobile number")]
        [RegularExpression(RegexPatterns.MobileNo,ErrorMessage ="Mobile number is invalid.")]
        [Required(ErrorMessage ="Mobile number is required.")]
        public string? MobileNo { get; set; }

        [BindProperty]
        public bool IsBulkRequest { get; set; }

        public bool IsGlobalAdmin { get; set; }

        public void OnGet(string? mobileNo)
        {
            this.MobileNo = mobileNo;
            IsGlobalAdmin = _appUser.Current.Role == ApplicationCore.Enums.UserRole.Admin;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (!IsBulkRequest)
                    return RedirectToPage("New", new { MobileNo });
                else 
                    return RedirectToPage("Bulk", new { MobileNo });
            }

            IsGlobalAdmin = _appUser.Current.Role == ApplicationCore.Enums.UserRole.Admin;
            return Page();
        }
    }
}