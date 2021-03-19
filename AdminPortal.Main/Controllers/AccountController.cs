using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Extensions;
using TKW.AdminPortal.Responses;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Enums;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private IAppUserService _appUser;

        public AccountController(IAppUserService appUser, IIdentityService identityService)
        {
            _appUser = appUser;
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            AuthenticateResult authenticateResult = await HttpContext.AuthenticateAsync();
            if (authenticateResult.Succeeded)
            {
                await _identityService.EndLoginSessionAsync(cancellationToken);
                HttpContext.SignOut();
            }
            return RedirectToPage("/Index");
        }

        //[Authorize]
        //[Route("~/ajax/search/account/users")]
        //[AcceptVerbs("GET", "POST")]
        //public async Task<JsonResult> Users(string q, int? page, CancellationToken cancellationToken)
        //{
        //    var users = await _accountService.SearchUsersAsync(q, page ?? 1, 10, cancellationToken);
        //    if (users == null) return null;
        //    return Json(new PagedJsonResponse<UserDOM>
        //    {
        //        Data = users,
        //        Page = users.PageNumber,
        //        Size = users.PageSize,
        //        Total = users.TotalItemCount
        //    });
        //}

        [HttpGet]
        [Authorize]
        public IActionResult SetFranchise(int id,string Return,CancellationToken cancellationToken)
        {
            if(_appUser.Current.Role==UserRole.Admin || _appUser.Current.Role == UserRole.TeleCaller)
            {
                if (id == 0)
                {
                    Response.Cookies.Delete(Cookies.Franchise);
                }
                else
                {
                    Response.Cookies.Append(Cookies.Franchise, id.ToString(), new CookieOptions
                    {
                        Expires = DateTime.MaxValue
                    });
                }
                HttpContext.Session.DeleteAuthTicket();
            }
            return Redirect(Return);
        }
    }
}