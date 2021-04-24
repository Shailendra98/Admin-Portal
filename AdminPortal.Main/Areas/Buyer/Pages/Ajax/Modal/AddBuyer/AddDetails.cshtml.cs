using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.Buyer.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Buyer.Pages.Ajax.Modal.AddBuyer
{
    public class AddDetailsModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IAreaQueries _areaQueries;
        private readonly IUserQueries _userQueries;
        private readonly IBuyerQueries _buyerQueries;
        private readonly IBuyerService _buyerService;
        private readonly IUserService _userService;

        public AddDetailsModel(IAppUserService appUser, IUserQueries userQueries, IBuyerQueries buyerQueries, IBuyerService buyerService, IUserService userService, IAreaQueries areaQueries)
        {
            _appUser = appUser;
            _userQueries = userQueries;
            _buyerQueries = buyerQueries;
            _buyerService = buyerService;
            _userService = userService;
            _areaQueries = areaQueries;
        }

        [BindProperty]
        [Required(ErrorMessage = "Buyer details is missing.")]
        public BuyerInputModel BuyerInputModel { get; set; }

        public string? ErrorMessage { get; set; }

        public bool IsDone { get; set; }

        public async Task OnGetAsync(string OwnerMobileNo, CancellationToken cancellationToken)
        {
            BuyerInputModel = new BuyerInputModel()
            {
                OwnerMobileNo = OwnerMobileNo,
                Address = new AdminPortal.ViewModels.AddressModel { IncludeNameMobileNo = false, OnlyLocalities = _appUser.Current.FranchiseId.HasValue, IncludeAddressType = false }
            };

            var u = await _userQueries.UserByMobileNumberAsync(OwnerMobileNo, cancellationToken);
            if (u != null)
            {
                BuyerInputModel.OwnerName = u.Name;
            }
        }


        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var buyer = await _buyerService.AddBuyerAsync(BuyerInputModel.OwnerName, BuyerInputModel.OwnerMobileNo, BuyerInputModel.FirmName, BuyerInputModel.GSTIN, _appUser.Current.FranchiseId!.Value, BuyerInputModel.Address.AddressLine, BuyerInputModel.Address.LocalityId.Value, BuyerInputModel.Address.Latitude.Value, BuyerInputModel.Address.Longitude.Value, cancellationToken);
                if (buyer.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else
                {
                    ErrorMessage = buyer.Error.Message;
                }
            }

            BuyerInputModel.Address.AddressTypes = Enumeration.GetAll<AddressType>().ToList();
            if (BuyerInputModel.Address.LocalityId.HasValue)
            {
                var locality = await _areaQueries.LocalityByIdAsync(BuyerInputModel.Address.LocalityId.Value, cancellationToken);
                BuyerInputModel.Address.LocalityName = locality.Name;
                BuyerInputModel.Address.LocalityLatitude = locality.Latitude;
                BuyerInputModel.Address.LocalityLongitude = locality.Longitude;
            }
            BuyerInputModel.Address.OnlyLocalities = _appUser.Current.FranchiseId.HasValue;
            BuyerInputModel.Address.IncludeNameMobileNo = false;
            BuyerInputModel.Address.IncludeAddressType = false;
            return Page();
        }

    }
}
