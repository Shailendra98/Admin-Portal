using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Utils;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AccountContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Pages.Ajax.Address_
{
    [Authorize]
    public class NewModel : PageModel
    {
        private IUserAddressQueries _userAddressQueries;
        private IUserService _UserService;
        private IAppUserService _appUser;
        private IAreaQueries _areaQueries;

        public NewModel(IUserAddressQueries userAddressQueries, IUserService userService, IAppUserService appUserService, IAreaQueries areaQueries)
        {
            _userAddressQueries = userAddressQueries;
            _UserService = userService;
            _appUser = appUserService;
            _areaQueries = areaQueries;
        }

        [BindProperty(SupportsGet = true)]

        public string ModalId { get; set; }

        [BindProperty(SupportsGet = true)]

        public int UserId { get; set; }

        public bool HideForm { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public AddressModel AddressModel { get; set; }

        public async Task OnGet()
        {
            AddressModel = new AddressModel
            {
                IncludeNameMobileNo = true,
                OnlyLocalities = _appUser.Current.FranchiseId.HasValue
            };
        }
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var addressin = await _UserService.AddUserAddressAsync(
                    UserId,
                    AddressModel.AddressLine,
                    AddressModel.LocalityId.GetValueOrDefault(),
                    AddressModel.AddressTypeId ?? AddressType.Home.Id,
                    AddressModel.Name,
                    AddressModel.MobileNo,
                    AddressModel.Latitude,
                    AddressModel.Longitude,
                    cancellationToken);

                if (addressin.IsSuccess)
                {
                    var address = await _userAddressQueries.UserAddressByIdAsync(addressin.Value.Id, cancellationToken);

                    return Content(@"<script>
                                        $('#" + ModalId + @"').find('.modal-content').trigger({
                                                type:'action.done',
                                                id:" + address.Id + @",
                                                html:'<address class=""m-b-none"">" + AddressUtils.GenerateHTML(address.AddressLine, address.Type.Name, address.LocalityName, address.CityName, address.StateName, address.Pincode.ToString(), address.Name, address.MobileNo) + @"</address>'
                                        });
                                    </script>");
                }

                if (addressin.Error is LocalityNotFoundError)
                    ModelState.AddModelError(nameof(AddressModel.LocalityId), addressin.Error.Message);
                else
                    ErrorMessage = addressin.Error.Message;
            
            }

            AddressModel.AddressTypes = Enumeration.GetAll<AddressType>().ToList();
            if (AddressModel.LocalityId.HasValue)
            {
                var locality = await _areaQueries.LocalityByIdAsync(AddressModel.LocalityId.Value, cancellationToken);
                AddressModel.LocalityName = locality.Name;
                AddressModel.LocalityLatitude = locality.Latitude;
                AddressModel.LocalityLongitude = locality.Longitude;
            }

            if (AddressModel.CityId.HasValue)
                AddressModel.CityName = (await _areaQueries.CityByIdAsync(AddressModel.CityId.Value, cancellationToken)).Name;

            AddressModel.IncludeNameMobileNo = true;
            AddressModel.OnlyLocalities = _appUser.Current.FranchiseId.HasValue;
            return Page();
        }
    }
}