using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;
using TKW.ApplicationCore.Contexts.AccountContext.Queries;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Pages.Ajax.Address_
{
    [Authorize]
    public class ListModel : PageModel
    {
        private IUserAddressQueries _userAddressQueries;
        private IUserService _UserService;
        private IAppUserService _appUser;

        public ListModel(IUserAddressQueries userAddressQueries, IUserService userService,IAppUserService appUserService) {
            _userAddressQueries = userAddressQueries;
            _UserService = userService;
            _appUser = appUserService;
        }

        public IEnumerable<UserAddressModel>  Addresses { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ModalId { get; set; }

        [FromRoute]
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if(_appUser.Current.FranchiseId.HasValue)
                Addresses = await _userAddressQueries.UserAddressesByUserIdAsync(UserId, _appUser.Current.FranchiseId.Value, cancellationToken);
            else
                Addresses = await _userAddressQueries.UserAddressesByUserIdAsync(UserId, cancellationToken);
        }
    }
}