using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal
{
    public class GSTInvoiceModel : PageModel
    {
        private ISellQueries _sellQueries;
        private IAppUserService _appUser;

        public GSTInvoiceModel(ISellQueries sellQueries, IAppUserService appUser)
        {
            _sellQueries = sellQueries;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet =true)]
        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public bool IsIGST { get; set; }

        public SellFullDOM sell { get; set; }

        public bool IsDone { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            sell = await _sellQueries.GetSellByIdAsync(Id, cancellationToken);
            if(sell == null || (_appUser.Current.FranchiseId.HasValue && sell.Warehouse.Franchise.Id != _appUser.Current.FranchiseId))
                return Content(ModalUtils.GenerateContent("GST Invoice", "<div class='alert alert-danger'>Sell does not exist.</div>", "").ToString());
            else if(sell.InvoiceNo.HasValue)
                return Content(ModalUtils.GenerateContent("GST Invoice", "<div class='alert alert-info'>Sell has already GST Invoice.</div>", "").ToString());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            sell = await _sellQueries.GetSellByIdAsync(Id, cancellationToken);
            if (sell == null || (_appUser.Current.FranchiseId.HasValue && sell.Warehouse.Franchise.Id != _appUser.Current.FranchiseId))
                return Content(ModalUtils.GenerateContent("GST Invoice", "<div class='alert alert-danger'>Sell does not exist.</div>", "").ToString());
            else if (sell.InvoiceNo.HasValue)
                return Content(ModalUtils.GenerateContent("GST Invoice", "<div class='alert alert-info'>Sell has already GST Invoice.</div>", "").ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    await _sellQueries.MakeGSTBillOfSellAsync(Id, IsIGST, cancellationToken);
                    IsDone = true;
                }
                catch(SellNotFoundException)
                {
                    return Content(ModalUtils.GenerateContent("GST Invoice", "<div class='alert alert-danger'>Sell does not exist.</div>","").ToString());
                }
            }
            return Page();
        }
    }
}
