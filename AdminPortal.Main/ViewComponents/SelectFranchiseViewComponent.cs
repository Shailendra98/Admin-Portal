using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;

namespace TKW.AdminPortal.ViewComponents
{
    public class SelectFranchiseViewComponent : ViewComponent
    {
        private readonly IFranchiseQueries _franchiseQueries;
        public SelectFranchiseViewComponent(IFranchiseQueries franchiseQueries)
        {
            _franchiseQueries = franchiseQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var franchises = await _franchiseQueries.AllFranchisesAsync(cancellationToken);
            return View(franchises);
        }
    }
}
