using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;

namespace TKW.AdminPortal.Areas.Request.Pages
{
    public class InvoiceModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;
        public InvoiceModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public RequestFullModel? Request { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            Request = await _requestQueries.FullRequestDetailsByIdAsync(Id, cancellationToken);
            if (Request == null || Request.Completion == null) return NotFound();
            return Page();
        }
    }
}
