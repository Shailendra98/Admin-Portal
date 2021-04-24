using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Incentive.ViewModels;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax.Modal
{
    public class AddRewardModel : PageModel
    {
        public readonly IFranchiseQueries _franchiseQueries;
        public readonly IIncentiveService _incentiveService;


        public AddRewardModel(IFranchiseQueries franchiseQueries,IIncentiveService incentiveService)
        {
            _incentiveService = incentiveService;
            _franchiseQueries = franchiseQueries;
        }

      //Task<Result> AddIncentiveTargetRewardAsync(int targetId, int reward, DateTime startDateTime, DateTime endDateTime, bool isGlobal, List<int>? franchiseIds = null, CancellationToken cancellationToken = default);

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }


        [BindProperty(SupportsGet =true)]
        public int Target { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IncentiveTypeName { get; set; }

        [BindProperty]
        public RewardInputModel RewardModel { get; set; }

        public bool IsDone { get; set; }
        public string Errormessage { get; set; }

        public MultiSelectList? FranchiseList { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _incentiveService.AddIncentiveTargetRewardAsync(Id, RewardModel.Reward, RewardModel.StartDateTime, RewardModel.EndDateTime, RewardModel.IsGlobal, RewardModel.FranchiseIds, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                }
                else
                {
                    Errormessage = result.Error.Message;
                }
            }
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", RewardModel.FranchiseIds);
            return Page();
        }
    }
}
