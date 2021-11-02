using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax
{
    public class UserListModel : PageModel
    {
        private IUserQueries _userQueries;
        public UserListModel(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }
        [BindProperty(SupportsGet = true)]
        public UserFilterSortModel Filter { get; set; }
        public PagedList<UserListItemModel> Users { get; set; }
        public async Task OnGetAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            Users = await _userQueries.FilteredAndSortedUsersAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
        public async Task OnPostAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            Users = await _userQueries.FilteredAndSortedUsersAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
    }
}
