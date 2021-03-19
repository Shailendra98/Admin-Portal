using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TKW.ApplicationCore.Contexts.PaymentContext.Services;

namespace TKW.AdminPortal.Controllers
{

    [Authorize]
    public class RequestController : Controller
    {
        private readonly IPaymentTransactionService _paymentTransactionService;

        public RequestController(IPaymentTransactionService paymentTransactionService)
        {
            _paymentTransactionService = paymentTransactionService;
        }

        [AcceptVerbs("POST")]
        [Route("~/Request/CheckPaymentStatus")]
        public async Task<IActionResult> CheckRequestPaymentStatus(long paymentTransactionId,CancellationToken cancellationToken)
        {
            var result = await _paymentTransactionService.GetRequestPaymentTransactionStatusAsync(paymentTransactionId, cancellationToken);
            if (result.IsSuccess)
                return Json(new { statusId = result.Value.Id });
            return Json(null);
            
        }
    }
}