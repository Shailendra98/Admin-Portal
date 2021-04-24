using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Purchase;
using TKW.ApplicationCore.Identity;
using TKW.Queries.Interfaces;
using System.ComponentModel;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    public class RequestsModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;

        public RequestsModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Tab { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool EndedSession { get; set; }
        
        public RequestCountsOfPickupSessionModel PickupSessionRequestCounts { get; set; }
        
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
           var requestCounts = await _requestQueries.RequestCountsOfPickupSessionsAsync(new List<int> { id }, cancellationToken);
           PickupSessionRequestCounts = requestCounts.FirstOrDefault() ?? new RequestCountsOfPickupSessionModel();
        }
    }
}
