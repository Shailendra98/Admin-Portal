using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax.Modal
{
    public class DeleteAddressModel : PageModel
    {
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public DeleteAddressModel(IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;

        }

        [BindProperty(SupportsGet = true)]
        public string? Tab { get; set; }

        public UserAddressModel? UserAddress { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            UserAddress = await _userAddressQueries.UserAddressByIdAsync(id, cancellationToken);
        }
    }
}
