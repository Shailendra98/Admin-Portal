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
using TKW.Queries.DTOs.Account;
using TKW.ApplicationCore.Contexts.AccountContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Pages.Ajax.Address
{
    [Authorize]
    public class EditModel : PageModel
    {
        private IUserAddressQueries _userAddressQueries;
        private IUserService _UserService;
        private IAppUserService _appUser;

        public EditModel(IUserAddressQueries userAddressQueries, IUserService userService, IAppUserService appUserService)
        {
            _userAddressQueries = userAddressQueries;
            _UserService = userService;
            _appUser = appUserService;
        }

        [FromRoute]
        [BindProperty(SupportsGet = true)]
        public long AddressId { get; set; }

        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public string ModalId { get; set; }

        [BindProperty]
        public AddressModel AddressModel { get; set; }

        public bool HideForm { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGet()
        {
             var address = await _userAddressQueries.UserAddressByIdAsync(AddressId);
            if (address == null)
            {
                ErrorMessage = "Address does not exist.";
                HideForm = true;
            }
            else { 
                AddressModel = new AddressModel
                {
                    IncludeNameMobileNo = true,
                    LocalityName = address.LocalityName,
                    CityName = address.CityName,
                    AddressLine = address.AddressLine,
                    AddressTypeId = address.Type.Id,
                    CityId = address.CityId,
                    LocalityId = address.LocalityId,
                    MobileNo = address.MobileNo,
                    Name = address.Name,
                    Latitude = address.Latitude,
                    Longitude = address.Longitude,
                    LocalityLatitude = address.LocalityLatitude,
                    LocalityLongitude = address.LocalityLongitude,
                    OnlyLocalities = _appUser.Current.FranchiseId.HasValue
                };
            }
        }
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if(ModelState.IsValid)
            {
              var editAddress  =    await _UserService.EditUserAddressAsync(
                    AddressId,
                    AddressModel.AddressLine,
                    AddressModel.LocalityId.Value,
                    AddressModel.AddressTypeId ?? AddressType.Home.Id,
                    AddressModel.Name,
                    AddressModel.MobileNo,
                    AddressModel.Latitude,
                    AddressModel.Longitude,
                    cancellationToken);

                if (editAddress.IsSuccess){

                    var updatedAddress = await _userAddressQueries.UserAddressByIdAsync(AddressId, cancellationToken);

                    return Content(@"<script>
                                        $('#" + ModalId + @"').find('.modal-content').trigger({
                                            type:'action.done',
                                            id:" + AddressId + @",
                                            html:'<address class=""m-b-none"">" + AddressUtils.GenerateHTML(updatedAddress.AddressLine, updatedAddress.Type.Name, updatedAddress.LocalityName, updatedAddress.CityName, updatedAddress.StateName, updatedAddress.Pincode.ToString(), updatedAddress.Name, updatedAddress.MobileNo) + @"</address>'
                                        });
                                </script>");
                }

                if (editAddress.Error is LocalityNotFoundError) {
                    ModelState.AddModelError(nameof(AddressModel.LocalityId), editAddress.Error.Message);
                }
                else
                {
                    ErrorMessage = editAddress.Error.Message;
                }
            }

            var address = await _userAddressQueries.UserAddressByIdAsync(AddressId, cancellationToken);
            AddressModel.LocalityName = address.LocalityName;
            AddressModel.CityName = address.CityName;
            AddressModel.LocalityLatitude = address.LocalityLatitude;
            AddressModel.LocalityLongitude = address.LocalityLongitude;
            AddressModel.IncludeNameMobileNo = true;
            AddressModel.OnlyLocalities = _appUser.Current.FranchiseId.HasValue;

            return Page();
        }
    }
}