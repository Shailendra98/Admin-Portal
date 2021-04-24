using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Incentive;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax
{
    public class IncentiveListModel : PageModel
    {
        public readonly IIncentiveQueries _incentiveQueries;
        public IncentiveListModel(IIncentiveQueries incentiveQueries)
        {
            _incentiveQueries = incentiveQueries;
        }
        public List<IncentiveWithTargetsAndRewardsModel> Incentives { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Incentives = await _incentiveQueries.AllIncentiveTypesWithTargetsAndRewardsAsync(cancellationToken);
        }
    }
}
