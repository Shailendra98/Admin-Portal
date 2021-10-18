using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;

namespace TKW.AdminPortal.Areas.UserProfile.Pages
{
    public class IndexModel : PageModel
    {
       
        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserFilterSortModel Filter { get; set; }
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {

        }
    }
}
