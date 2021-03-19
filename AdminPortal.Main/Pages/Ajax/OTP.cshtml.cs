using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Extensions;
using TKW.ApplicationCore.Contexts.AccountContext.Errors;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Enums;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Pages.Ajax
{
    public class OTPModel : PageModel
    {
        private readonly IIdentityService _identityService;
        public OTPModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [BindProperty]
        [HiddenInput]
        public string? ReturnUrl { get; set; }

        [BindProperty]
        [Required]
        [HiddenInput]
        [Display(Name = "Mobile number")]
        public string MobileNo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "OTP is required.")]
        public string OTP { get; set; }

        public void OnGet(string MobileNo, string ReturnUrl)
        {
            this.MobileNo = MobileNo;
            this.ReturnUrl = ReturnUrl;
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
               
                var r = await _identityService.GenerateLoginSessionAsync(MobileNo, OTP, PasswordType.OTP, SourceApp.AdminPortal, null, cancellationToken);
                if (r.IsSuccess)
                {
                    ClaimsPrincipal? claimsPrincipal;
                    var hasFranchiseCookie = Request.Cookies.TryGetValue(Cookies.Franchise, out string franchise);
                    if (hasFranchiseCookie && int.TryParse(franchise, out int franchiseId))
                        claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(r.Value.sessionToken, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie, franchiseId);
                    else
                        claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(r.Value.sessionToken, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie);
                    HttpContext.SignIn(r.Value.sessionToken, claimsPrincipal);
                    return Content("<script>window.location.href='/Dashboard';</script>");
                }
                if (r.Error is IncorrectPasswordError) return Content("OTP is incorrect.");
                string errormessage = r.Error.Message;
                return Content("<script>$(function(){$('#SignContent').html('<div class=\"panel-body text-center\"><div class=\"alert alert-danger\">" + errormessage + "</div>');})</script>");
            }
            return Content("OTP is invalid.");
        }
    }
}