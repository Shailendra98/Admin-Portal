using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Requests
{
    public class CancelledModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;
        public CancelledModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<CanceledRequestModel> Requests { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Requests = await _requestQueries.CanceledRequestOfPickupSessionAsync(Id, cancellationToken);
        }
    }
}
