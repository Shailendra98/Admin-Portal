using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Address.Exceptions;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.Franchise.Services;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax.Modal
{
    public class AddModel : PageModel
    {
        private IFranchiseService _franchiseService;

        public AddModel(IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
        }

        [BindProperty]
        public FranchiseModel FranchiseModel { get; set; }

        public bool IsDone { get; set; }

        public void OnGet()
        {
            FranchiseModel = new FranchiseModel
            {
                Address = new AdminPortal.ViewModels.AddressModel
                {
                    IncludeNameMobileNo = false,
                    OnlyLocalities = false,
                }
            };
        }

        public async Task OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _franchiseService.AddFranchiseAsync(new TKW.Franchise.DTOs.Input.FranchiseDIM {
                        ContactNo=FranchiseModel.ContactNo,
                        Name=FranchiseModel.Name,
                        IsActive=FranchiseModel.IsActive,
                        Address = new Address.DTOs.Input.AddressDIM { 
                            AddressLine= FranchiseModel.Address.AddressLine,
                            Landmark= FranchiseModel.Address.Landmark,
                            LocalityId=FranchiseModel.Address.LocalityId.Value,
                        }
                    }, cancellationToken);
                    IsDone = true;
                }
                catch (LocalityNotFoundException)
                {
                    ModelState.AddModelError(nameof(FranchiseModel.Address.LocalityId), "Locality is invalid.");
                }
                if (!IsDone)
                {
                    FranchiseModel.Address.OnlyLocalities = false;
                    FranchiseModel.Address.IncludeNameMobileNo = false;
                }
            }
        }
    }
}
