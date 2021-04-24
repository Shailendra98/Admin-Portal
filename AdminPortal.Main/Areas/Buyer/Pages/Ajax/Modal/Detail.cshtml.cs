using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Buyer.ViewModels;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;

namespace TKW.AdminPortal.Areas.Buyer.Pages.Ajax.Modal
{
    public class DetailModel : PageModel
    {
        private readonly IBuyerQueries _buyerQueries;

        public DetailModel(IBuyerQueries buyerQueries)
        {
            _buyerQueries = buyerQueries;
        }

        [BindProperty(SupportsGet =true)]
        public int BuyerId { get; set; }

        public BuyerWithAddressModel Buyer { get; set; }
      
        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
               Buyer = await _buyerQueries.BuyerAsync(BuyerId, cancellationToken);
               if (Buyer == null) return Content(Utils.ModalUtils.GenerateContent("Buyer Details", "<div class='alert alert-danger'>Buyer does not exist any longer.</div>", "").ToString());

                return Page();
        }
    }
}
