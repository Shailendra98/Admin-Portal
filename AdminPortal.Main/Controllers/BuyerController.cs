using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TKW.AdminPortal.Responses;
using TKW.Sell.DTOs.Output;
using TKW.Sell.Services;

namespace TKW.AdminPortal.Controllers
{
    public class BuyerController : Controller
    {

        private ISellService _sellService;

        public BuyerController(ISellService sellService)
        {
            _sellService = sellService;
        }


        [Authorize]
        [Route("~/ajax/search/buyers")]
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> Search(string q,int? page,CancellationToken cancellationToken)
        {
            var buyers = await _sellService.SearchBuyersAsync(q, page ?? 1, 10, cancellationToken);
            if (buyers == null) return null;
            return Json(new PagedJsonResponse<BuyerDOM>
            {
                Data = buyers,
                Page = buyers.PageNumber,
                Size = buyers.PageSize,
                Total = buyers.TotalItemCount
            });
        }
    }
}