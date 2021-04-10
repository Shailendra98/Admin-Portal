using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Buyer.ViewModels;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Buyer.Pages.Ajax.Modal
{
    public class EditModel : PageModel
    {

        private readonly IAppUserService _appUser;
        private readonly IBuyerService _buyerService;
        private readonly IBuyerQueries _buyerQueries;

        public EditModel(IAppUserService appUser,IBuyerService buyerService,IBuyerQueries buyerQueries)
        {
            _appUser = appUser;
            _buyerService = buyerService;
            _buyerQueries = buyerQueries;
        }

        [BindProperty(SupportsGet =true)]
        public int BuyerId { get; set; }

        [BindProperty(SupportsGet =true)]
        [Required]
        public BuyerInputModel BuyerInputModel { get; set; }

        public bool IsDone { get; set; }

        public string? ErrorMessage { get; set; }


        

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var b = await _buyerQueries.BuyerAsync(BuyerId,cancellationToken);
            if (b == null)
            {
                return Content(Utils.ModalUtils.GenerateContent("Edit Details", "<div class='alert alert-danger'>Buyer does not exist any longer.</div>", "").ToString());
            }

            BuyerInputModel = new BuyerInputModel()
                {
                    OwnerName = b.OwnerName,
                    OwnerMobileNo = b.OwnerMobileNo,
                    FirmName = b.FirmName,
                    GSTIN = b.GSTIN,
                    Address = new AdminPortal.ViewModels.AddressModel
                    {
                        IncludeNameMobileNo = false,
                        OnlyLocalities = _appUser.Current.FranchiseId.HasValue,
                        IncludeAddressType = false,
                        AddressLine = b.AddressLine,
                        Latitude = b.Latitude,
                        Longitude = b.Longitude,
                        LocalityName = b.LocalityName,
                        LocalityId = b.LocalityId
                    }
                };

            return Page();
        } 

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
           
            if (ModelState.IsValid)
            {
                var buyer = await _buyerService.EditBuyerAsync(BuyerId,BuyerInputModel.OwnerName, BuyerInputModel.OwnerMobileNo, BuyerInputModel.FirmName, BuyerInputModel.GSTIN, _appUser.Current.FranchiseId!.Value, BuyerInputModel.Address.AddressLine, BuyerInputModel.Address.LocalityId.Value, BuyerInputModel.Address.Latitude.Value, BuyerInputModel.Address.Longitude.Value, BuyerInputModel.IsActive,cancellationToken);
                if (buyer.IsSuccess)
                {
                    IsDone = true;
                }
                else
                {
                    ErrorMessage = buyer.Error.Message;
                }      
            }

            BuyerInputModel.Address.OnlyLocalities= true ;
            BuyerInputModel.Address.IncludeNameMobileNo = false ;
            BuyerInputModel.Address.IncludeAddressType = false ;
            return Page();
        }
    }
}
