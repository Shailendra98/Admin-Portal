using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Constants;
using TKW.ApplicationCore.Enums;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Employee.Pages
{
    [Authorize(Policy = AuthorizationPolicies.GlobalAdminRoles)]
    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUser;

        public IndexModel(IAppUserService appUser)
        {
            _appUser = appUser;
        }
        public bool IsFranchise { get; set; }
        
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
        }
    }
}
