using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.ViewComponents.Models;
using TKW.AdminPortal.ViewModels;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.ViewComponents
{
    public class SelectAddressViewComponent : ViewComponent
    {
        private readonly IUserAddressQueries _userAddressQueries;
        //private readonly IAppUserService _appUser;

        public SelectAddressViewComponent(IUserAddressQueries userAddressQueries)//, IAppUserService appUser)
        {
            _userAddressQueries = userAddressQueries;
           // _appUser = appUser;
        }

        public async Task<IViewComponentResult> InvokeAsync(long Id, string InputName, string ModalId, CancellationToken cancellationToken)
        {
            try
            {
                var address = await _userAddressQueries.UserAddressByIdAsync(Id, cancellationToken: cancellationToken);
                if (address != null)
                    return View(new SelectAddressModel(address, InputName, ModalId));
            }
            catch
            {
            }
            var model = new AddressModel
            {
                IncludeNameMobileNo = false,
      //          OnlyLocalities = _appUser.Current.FranchiseId.HasValue
            };
            return View("Input", model);

        }
    }
}
