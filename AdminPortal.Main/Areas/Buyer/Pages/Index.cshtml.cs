using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Buyer.Pages
{
    public class IndexModel : PageModel
    {

        public readonly IAppUserService _appUser;

        public IndexModel(IAppUserService appUser)
        {
            _appUser = appUser;
        }

        public bool IsFranchise { get; set; }
        public void OnGet()
        {
            IsFranchise = _appUser.Current.FranchiseId != null;

        }
    }
}
