using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Extensions
{
    public static class HttpContextExtension
    {
        public static void SignIn(this HttpContext Context, string sessionToken, ClaimsPrincipal? claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                Context.Response.Cookies.Append("User", sessionToken);
                Context.Session.SetAuthTicket(new AuthenticationTicket(claimsPrincipal, AuthScheme.SessionToken_Cookie));
                ApplyHeaders(Context);
            }
        }
        public static void SignOut(this HttpContext Context)
        {
            Context.Response.Cookies.Delete("User");
            Context.Session.DeleteUser();
            ApplyHeaders(Context);
        }
        private static void ApplyHeaders(HttpContext Context)
        {
            Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
            Context.Response.Headers[HeaderNames.Expires] = "Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }
}
