using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Constants;
using TKW.ApplicationCore.Enums;

namespace TKW.AdminPortal.Areas.Franchise.Pages
{

    [Authorize(Policy = AuthorizationPolicies.GlobalAdminRoles)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
