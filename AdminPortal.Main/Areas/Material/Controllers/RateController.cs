using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.AdminPortal.Areas.Material.ViewModels;

namespace TKW.AdminPortal.Areas.Material.Controllers
{
    [Authorize]
    public class RateController : Controller
    {
        private IMaterialService _materialService;
        private IAppUserService _appUser;

        public RateController(IMaterialService materialService, IAppUserService appUser, IFranchiseQueries franchiseQueries)
        {
            _materialService = materialService;
            _appUser = appUser;
        }

        [HttpPost("~/Material/Rate")]
        public async Task<IActionResult> UpdateRateOfFranchise(PurchaseMaterialUpdateModel model, CancellationToken cancellationToken)
        {
            var franchiseid = _appUser.Current.FranchiseId;
            if (franchiseid.HasValue)
            {
                if (ModelState.IsValid)
                {
                    await _materialService.UpdatePurchaseMaterialRateOfFranchiseAsync(model.PurchaseMaterialId.Value, _appUser.Current.FranchiseId.Value, model.Rate.Value, model.MinRate.Value, model.MaxRate.Value, model.IncludeInRateList, cancellationToken);
                    return Json(true);
                }
                return Json(false);
            }
            return Forbid();
        }
    }
}