using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Material.ViewModels;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Material;
using TKW.ApplicationCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using Microsoft.AspNetCore.Authorization;

namespace TKW.AdminPortal.Areas.Material.Pages.Ajax.Modal
{
    [Authorize]
    public class UpdateRateListModel : PageModel
    {
        private readonly IMaterialService _materialService;
        private readonly IAppUserService _appUser;
        private readonly IMaterialQueries _materialQueries;

        public UpdateRateListModel(IMaterialQueries materialQueries, IAppUserService appUser, IMaterialService materialService)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
            _materialService = materialService;
        }

        [BindProperty(SupportsGet = true)]
        public PurchaseMaterialUpdateModel Material { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsDone { get; set; }

        public async Task OnPostAsync(CancellationToken cancellationToken)
        {
            var franchiseid = _appUser.Current.FranchiseId;
            if (franchiseid.HasValue)
            {
                if (ModelState.IsValid)
                {
                    var result = await _materialService.UpdatePurchaseMaterialRateOfFranchiseAsync(Material.PurchaseMaterialId.Value, 
                        _appUser.Current.FranchiseId.Value,
                        Material.Rate.Value,
                        Material.MinRate.Value,
                        Material.MaxRate.Value,
                        Material.IncludeInSellerRateList,
                        Material.IncludeInPickupExecutiveRateList,
                        cancellationToken);
                    if (result.IsSuccess)
                    {
                        IsDone = true;
                    }
                    else
                    {
                        ErrorMessage = result.Error.Message;
                    }
                }
            }
        }
    }
}