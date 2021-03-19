using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;

namespace TKW.AdminPortal.Areas.Request.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;

        public DetailsModel(IRequestQueries requestQueries)
        {
            _requestQueries = requestQueries;
        }

        public RequestFullModel RequestModel { get; set; }

        public async Task OnGetAsync(long Id, CancellationToken cancellationToken)
        {
            RequestModel = await _requestQueries.FullRequestDetailsByIdAsync(Id, cancellationToken);
        }
    }
}