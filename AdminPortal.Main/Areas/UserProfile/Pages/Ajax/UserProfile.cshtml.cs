using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.UserProfile.Pages.Ajax
{
    public class UserProfileModel : PageModel
    {
        private IUserQueries _userQueries;
        private IUserAddressQueries _userAddressQueries;

        public UserProfileModel(IUserQueries userQueries, IUserAddressQueries userAddressQueries)
        {
            _userQueries = userQueries;
            _userAddressQueries = userAddressQueries;

        }


        public UserAddressModel? UserAddress { get; set; }

        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            UserAddress = await _userAddressQueries.UserAddressByIdAsync(id, cancellationToken);
        }
    }
}
