using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Incentive.ViewModels;
using TKW.Queries.DTOs.Incentive;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Incentive.Pages
{
    public class IndexModel : PageModel
    {
        public readonly IIncentiveQueries _incentiveQueries;
        private readonly IAppUserService _appUser;
       

        public IndexModel(IIncentiveQueries incentiveQueries,IAppUserService appUser)
        {
            _incentiveQueries = incentiveQueries;
            _appUser = appUser;
        }
        public List<IncentiveWithTargetsAndRewardsModel> Incentives { get; set; }
        
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Incentives =await _incentiveQueries.AllIncentiveTypesWithTargetsAndRewardsAsync(cancellationToken);
        }
    }
}
