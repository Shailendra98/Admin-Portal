using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Extensions;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public IndexModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [BindProperty]
        [HiddenInput]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Mobile number/Username is required.")]
        public string? MobileNo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            ReturnUrl = returnUrl;
            var AuthResult = await HttpContext.AuthenticateAsync();
            if (AuthResult.Succeeded)
            {
                if (string.IsNullOrEmpty(ReturnUrl)) return RedirectToPage("Dashboard");
                else return Redirect(ReturnUrl);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {

            if (ModelState.IsValid)
            {
                
                var r = await _identityService.GenerateLoginSessionAsync(MobileNo, Password, ApplicationCore.Enums.PasswordType.Normal, SourceApp.AdminPortal, cancellationToken: cancellationToken);
                if (r.IsSuccess)
                {
                    ClaimsPrincipal? claimsPrincipal;
                    var hasFranchiseCookie = Request.Cookies.TryGetValue(Cookies.Franchise, out string franchise);
                    if (hasFranchiseCookie && int.TryParse(franchise, out int franchiseId))
                        claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(r.Value.sessionToken, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie, franchiseId);
                    else
                        claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(r.Value.sessionToken, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie);
                    HttpContext.SignIn(r.Value.sessionToken, claimsPrincipal);
                    return Content("<script>window.location.href='/';</script>");
                }
                string errorMessage = r.Error.Message;
                return Content("<div class='alert alert-danger'>" + errorMessage + "</div>");
            }
            return Content("<div class='alert alert-danger'>" + string.Join("<br/>", ModelState.SelectMany(m => m.Value.Errors.Select(a => a.ErrorMessage))) + "</div>");
        }

    }
}
