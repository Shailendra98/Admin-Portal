using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Incentive.ViewModels;

using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Contexts.IncentiveContext.DTOs;
using TKW.ApplicationCore.Contexts.IncentiveContext.Queries;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax.Modal
{
    public class EditRewardModel : PageModel
    {
        public readonly IFranchiseQueries _franchiseQueries;
        public readonly IIncentiveService _incentiveService;
        public readonly IIncentiveQueries _incentiveQueries;

        public EditRewardModel(IFranchiseQueries franchiseQueries, IIncentiveService incentiveService,IIncentiveQueries incentiveQueries)
        {
            _incentiveService = incentiveService;
            _incentiveQueries = incentiveQueries;
            _franchiseQueries = franchiseQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int TargetId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RewardId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string IncentiveTypeName { get; set; }

        public int TargetValue { get; set; }

        [BindProperty]
        public RewardInputModel RewardModel { get; set; }
        public bool IsDone { get; set; }
        public string Errormessage { get; set; }

        public MultiSelectList? FranchiseList { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {

            var r =await _incentiveQueries.GetIncentiveRewardAsync(RewardId, cancellationToken);
            if(r == null)
            {
               return Content(Utils.ModalUtils.GenerateContent("Edit Employee", "<div class='alert alert-danger'>Reward does not exist any longer.</div>", "").ToString());
            }
            RewardModel = new RewardInputModel() {
                    Reward = r.Reward,
                    IsGlobal = r.IsGlobal,
                    StartDateTime = r.StartDate,
                    EndDateTime = r.EndDate,
                    FranchiseIds = GetFranchiseIds(r.Franchises),
             };
            TargetValue = r.Target.Target;
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name",r.Franchises.Select(m => m.Id));
            return Page();

        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _incentiveService.EditIncentiveTargetRewardAsync(TargetId, RewardId, RewardModel.Reward, RewardModel.StartDateTime, RewardModel.EndDateTime, RewardModel.IsGlobal, RewardModel.FranchiseIds, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                }
                else
                {
                    Errormessage = result.Error.Message;
                }
            }
             var r =await _incentiveQueries.GetIncentiveRewardAsync(RewardId, cancellationToken);
            if(r != null) TargetValue = r.Target.Target;
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", RewardModel.FranchiseIds);
            return Page();
        }
        public List<int> GetFranchiseIds(List<FranchiseModel> franchises)
        {
            List<int> Ids = new List<int>();
            foreach (var i in franchises)
            {
                Ids.Add(i.Id);
            }
            return Ids;
        }
    }
    
}
