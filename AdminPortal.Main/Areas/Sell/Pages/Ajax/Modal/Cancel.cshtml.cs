using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.Shared.Enumerations;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal
{
    public class CancelModel : PageModel
    {
        private readonly ISellQueries _sellQueries;
        private readonly ISellService _sellService;

        public CancelModel(ISellService sellService,ISellQueries sellQueries)
        {
            _sellService = sellService;
            _sellQueries = sellQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty]
        [Display(Name = "Reason")]
        [Required(ErrorMessage = "Reason is required.")]
        public int? ReasonId { get; set; }

        public List<SellCancellationReasonModel>? Reasons { get; set; }

        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Reasons = await _sellQueries.SellCancellationReasonAsync(cancellationToken);
            IsDone = false;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _sellService.CancelSellAsync(Id, ReasonId.Value, SourceApp.AdminPortal, cancellationToken);
                if (result.IsSuccess /*|| result.Error is RequestAlreadyCancelledError*/)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }

            IsDone = false;
            return Page();
        }
    }
}
