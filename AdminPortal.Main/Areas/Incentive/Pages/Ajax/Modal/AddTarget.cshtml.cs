using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TKW.ApplicationCore.Contexts.ExpenseContext.DTOs;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Incentive.ViewModels;
using System.Threading;
using TKW.ApplicationCore.Contexts.IncentiveContext.Services;
using TKW.ApplicationCore.Contexts.IncentiveContext.Queries;
using TKW.ApplicationCore.Contexts.IncentiveContext.Aggregates;
using TKW.ApplicationCore.Contexts.IncentiveContext.DTOs;
using System.ComponentModel.DataAnnotations;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Incentive.Pages.Ajax.Modals
{
    public class AddTargetModel : PageModel
    {
        public readonly IFranchiseQueries _franchiseQueries;
        public readonly IIncentiveService _incentiveService;
        public readonly IIncentiveQueries _incentiveQueries;

        public AddTargetModel( IFranchiseQueries franchiseQueries,IIncentiveService incentiveService,IIncentiveQueries incentiveQueries)
        {
            _franchiseQueries = franchiseQueries;
            _incentiveService = incentiveService;
            _incentiveQueries = incentiveQueries;
        }

        [BindProperty]
        public List<int>? SelectedFranchiseIds { get; set; }
        public MultiSelectList? FranchiseList { get; set; }

        [BindProperty(SupportsGet =true)]
        public int IncentiveTypeId { get; set; }

        [BindProperty(SupportsGet =true)]
        public string IncentiveTypeName { get; set; }

        [BindProperty]
        [Display(Name ="Target")]
        [Required(ErrorMessage = "Target is Required")]
        public int? TargetValue { get; set; }

        [BindProperty]
        [Display(Name ="Default Reward")]
        [Required(ErrorMessage = "Default Reward is Required")]
        public int? DefaultReward { get; set; }

        [BindProperty]
        public bool IsGlobal { get; set; }

        public bool IsDone { get; set; }
        public string Errormessage { get; set; }

        public async Task OnGetAsync( CancellationToken cancellationToken)
        {
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name");
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result =await _incentiveService.AddIncentiveTargetAsync(Enumeration.FromValue<IncentiveType>(IncentiveTypeId),(int)TargetValue,(int)DefaultReward,IsGlobal,SelectedFranchiseIds,cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else
                {
                    Errormessage = result.Error.Message;
                }
            }
            FranchiseList = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", SelectedFranchiseIds);
            return Page();
        }
    }
}


