using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages
{
    public class DetailsModel : PageModel
    {
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public DetailsModel(IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;
        }


        public UserModel? Users { get; set; }
        public UserAddressModel? Address{get; set;}

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            Users = await _userQueries.UserByIdAsync(id, cancellationToken);
            Address = await _userAddressQueries.UserAddressByIdAsync(id, cancellationToken);

        }
    }
}
