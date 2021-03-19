using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Address.Exceptions;
using TKW.Address.Services;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.AdminPortal.Utils;
using TKW.Constants;
using TKW.Franchise.Exceptions;
using TKW.Franchise.Services;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax.Modal
{
    [Authorize(Roles=Role.Admin)]
    public class EditModel : PageModel
    {
        private IFranchiseService _franchiseService;
        private IAddressService _addressService;

        public EditModel(IFranchiseService franchiseService,IAddressService addressService)
        {
            _franchiseService = franchiseService;
            _addressService = addressService;
        }

        [BindProperty(SupportsGet = true)]
        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public FranchiseModel FranchiseModel { get; set; }

        public bool IsDone { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var franchise = await _franchiseService.GetFranchiseByIdAsync(Id, cancellationToken);
            if (franchise == null) return Content(ModalUtils.GenerateContent("Edit Franchise", "<div class='alert alert-danger'>Franchise does not exist.</div>", "").ToString());
            FranchiseModel = new FranchiseModel
            {
                Name=franchise.Name,
                ContactNo=franchise.ContactNo,
                IsActive=franchise.IsActive,
                Address = new AdminPortal.ViewModels.AddressModel
                {
                    AddressLine=franchise.Address?.AddressLine,
                    CityId=franchise.Address?.Locality?.City?.Id,
                    CityName=franchise.Address?.Locality?.City?.Name,
                    LocalityName=franchise.Address?.Locality?.Name,
                    LocalityId=franchise.Address?.Locality?.Id,
                    Landmark=franchise.Address?.Landmark,
                    OnlyLocalities = false,
                    IncludeNameMobileNo = false,
                }
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _franchiseService.EditFranchiseAsync(Id, new TKW.Franchise.DTOs.Input.FranchiseDIM
                    {
                        Name=FranchiseModel.Name,
                        Address=new Address.DTOs.Input.AddressDIM { 
                            AddressLine=FranchiseModel.Address.AddressLine,
                            Landmark=FranchiseModel.Address.Landmark,
                            LocalityId=FranchiseModel.Address.LocalityId.Value,
                        },
                        ContactNo=FranchiseModel.ContactNo,
                        IsActive=FranchiseModel.IsActive
                    }, cancellationToken);
                    IsDone = true;
                }
                catch(FranchiseNotFoundException)
                {
                    return Content(ModalUtils.GenerateContent("Edit Franchise", "<div class='alert alert-danger'>Franchise does not exist.</div>", "").ToString());
                }
                catch (LocalityNotFoundException)
                {
                    ModelState.AddModelError(nameof(FranchiseModel.Address.LocalityId), "Locality is invalid.");
                }
            }
            if (!IsDone)
            {
                FranchiseModel.Address.IncludeNameMobileNo = false;
                FranchiseModel.Address.OnlyLocalities = false;
                if (FranchiseModel.Address.LocalityId.HasValue)
                {
                    var locality = await _addressService.GetLocalityByIdAsync(FranchiseModel.Address.LocalityId.Value, cancellationToken);
                    FranchiseModel.Address.CityName = locality?.City?.Name;
                    FranchiseModel.Address.LocalityName = locality?.Name;
                    FranchiseModel.Address.CityId = locality?.City?.Id;
                }
            }
            return Page();
        }
    }
}
