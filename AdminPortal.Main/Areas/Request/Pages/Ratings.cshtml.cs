using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages
{
    public class RatingsModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;

        public RatingsModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        [BindProperty]
        public RequestRatingFilterSortModel Filter { get; set; }

        public List<FeedbackOptionModel> FeedbackOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FeedbackOptions = await _requestQueries.FeedbackOptionsAsync(cancellationToken);
        }
        public void Post()
        {

        }
    }
}
