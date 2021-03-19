using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.MaterialContext.DTOs;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;

namespace TKW.AdminPortal.Controllers
{
    [Authorize]
    public class MaterialController : Controller
    {
        private readonly IMaterialQueries _materialQueries;

        public MaterialController(IMaterialQueries materialQueries)
        {
            _materialQueries = materialQueries;
        }
        [HttpGet]
        [Route("~/ajax/materials/pickup")]
        public async Task<IActionResult> PurchaseMaterials(CancellationToken cancellationToken)
        {
            List<PurchaseMaterialModel> materials = await _materialQueries.ActivePurchaseMaterialsAsync(cancellationToken);
            return Json(materials);
        }

        [HttpGet]
        [Route("~/ajax/materials/sell")]
        public async Task<IActionResult> SellMaterials(CancellationToken cancellationToken)
        {
            List<SellMaterialModel> materials = await _materialQueries.ActiveSellMaterialsAsync(cancellationToken);
            return Json(materials);
        }
    }
}