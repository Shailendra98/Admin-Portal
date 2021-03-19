using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TKW.AdminPortal.Constants;
using TKW.AdminPortal.Extensions;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Security.Authentication
{
    public class SessionToken_CookieAuthenticationHandler : AuthenticationHandler<SessionTokenAuthenticationOptions>
    {
        private IHttpContextAccessor _httpContext;
        private IIdentityService _identityService;

        public SessionToken_CookieAuthenticationHandler(IOptionsMonitor<SessionTokenAuthenticationOptions> _options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IIdentityService identityService, IHttpContextAccessor httpContext) : base(_options, logger, encoder, clock)
        {
            _identityService = identityService;
            _httpContext = httpContext;
        }
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/AccessDenied");
            return Task.CompletedTask;
        }
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/");
            return Task.CompletedTask;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authTicket = _httpContext.HttpContext.Session.GetAuthTicket();

            if (authTicket == null)
            {
                if (!Request.Cookies.ContainsKey(Cookies.Auth))
                    AuthenticateResult.Fail("AuthCookie is missing.");

                if (Request.Cookies.TryGetValue(Cookies.Auth, out string session_token))
                {
                    try
                    {
                        ClaimsPrincipal claimsPrincipal;
                        var hasFranchiseCookie = Request.Cookies.TryGetValue(Cookies.Franchise, out string franchise);
                        if (hasFranchiseCookie && int.TryParse(franchise, out int franchiseId))
                        {
                            claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(session_token, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie, franchiseId);
                        }
                        else
                        {
                            claimsPrincipal = await _identityService.GetClaimsPrincipalBySessionTokenAsync(session_token, SourceApp.AdminPortal, AuthScheme.SessionToken_Cookie);
                        }

                        authTicket = new AuthenticationTicket(claimsPrincipal, AuthScheme.SessionToken_Cookie);
                        Context.Session.SetAuthTicket(authTicket);
                    }
                    catch (Exception)
                    {
                        return AuthenticateResult.Fail("Session token is invalid.");
                    }
                }
                else
                {
                    return AuthenticateResult.Fail("AuthCookie is missing.");
                }
            }
            else
            {
                _identityService.SetAppUser(authTicket.Principal);
            }
            return AuthenticateResult.Success(authTicket);
        }
    }
}

