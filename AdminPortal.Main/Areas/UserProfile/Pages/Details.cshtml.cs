using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages
{
    public class DetailsModel : PageModel
    {
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;
        private readonly IRequestQueries _requestQueries;


        public DetailsModel(IUserQueries userQueries, IUserAddressQueries userAddressQueries, IRequestQueries requestQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;
            _requestQueries = requestQueries;

        }


        public UserModel? User { get; set; }
        public List<UserAddressModel> Addresses { get; set; }
        public ContributionModel Impact { get; private set; }
        public long Id { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            User = await _userQueries.UserByIdAsync(id, cancellationToken);
            Addresses = await _userAddressQueries.UserAddressesByUserIdAsync(id, cancellationToken);
            Impact = await _requestQueries.ContributionOfSellerAsync(id, cancellationToken);
        }
    }
}
