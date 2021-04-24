using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Incentive;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax.Modal
{
    public class RemoveRewardModel : PageModel
    {
        public readonly IIncentiveService _incentiveService;
        public readonly IIncentiveQueries _incentiveQueries;

        public RemoveRewardModel(IIncentiveService incentiveService,IIncentiveQueries incentiveQueries)
        {
            _incentiveService = incentiveService;
            _incentiveQueries = incentiveQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int TargetId { get; set; }

        [BindProperty(SupportsGet =true)]
        public int RewardId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IncentiveTypeName { get; set; }

        public int TargetValue { get; set; }
        public IncentiveRewardWithTargetModel RewardData { get; set; }
        public bool IsDone { get; set; }
        public string Errormessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var e = await _incentiveQueries.GetIncentiveRewardAsync(RewardId, cancellationToken);
            if(e == null) return Content(Utils.ModalUtils.GenerateContent("Remove Reward", "<div class='alert alert-danger'>Reward does not exist any longer.</div>", "").ToString());
            RewardData = new IncentiveRewardWithTargetModel()
            {
                Reward = e.Reward,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Target =e.Target,
            };
            TargetValue = RewardData.Target.Target;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _incentiveService.RemoveIncentiveTargetRewardAsync(TargetId, RewardId, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                }
                else
                {
                    Errormessage = result.Error.Message;
                }
            }
            return Page();
        }
    }
}
