using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Material;
using TKW.Queries.Interfaces;

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

        [HttpGet]
        [Route("~/ajax/materials/sell/{unsegregatedMaterialId}")]
        public async Task<IActionResult> SellMaterials(int unsegregatedMaterialId, CancellationToken cancellationToken)
        {
            var unsegregatedMaterial = await _materialQueries.PurchaseStockMaterialbyIdAsync(unsegregatedMaterialId, cancellationToken);
            if (unsegregatedMaterial == null) return Json(null);
            List<SellMaterialModel> materials = unsegregatedMaterial!.SellMaterials;
            return Json(materials);
        }
    }
}