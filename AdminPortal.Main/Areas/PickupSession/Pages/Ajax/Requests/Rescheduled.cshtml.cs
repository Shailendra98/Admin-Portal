using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Requests
{
    public class RescheduledModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;
        public RescheduledModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public List<RescheduledRequestModel> Requests { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Requests = await _requestQueries.PendingRescheduledRequestsOfPickupSessionAsync(Id, cancellationToken);
        }
    }
}
