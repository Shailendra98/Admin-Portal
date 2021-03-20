using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IAppUserService _appUser;

        public DashboardModel(IAppUserService appUser)
        {
            _appUser = appUser;
        }

        public bool IsFranchise { get; set; }
        public void OnGet()
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
        }
    }
}
