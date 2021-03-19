using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.DTOs;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Controllers
{
    public class OTPController : Controller
    {

        private readonly IIdentityService _identityService;

        public OTPController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendOTPDTO dto, [FromRoute]string ReturnUrl, CancellationToken cancellationToken)
        {
               var result = await _identityService.GenerateAndSendOTPAsync(dto.MobileNo, SourceApp.AdminPortal, cancellationToken);
                if(result.IsSuccess) return RedirectToPage("/Ajax/OTP", new { dto.MobileNo, ReturnUrl });

            string errorMessage = result.Error.Message;

            return Content("<div class='panel-body text-center'><div class='alert alert-danger'>" + errorMessage + "</div></div>");
        }
    }
}