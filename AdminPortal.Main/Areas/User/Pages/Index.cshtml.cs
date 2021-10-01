using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Payment;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.User.Pages
{
    public class IndexModel : PageModel
    {
        private Queries.Interfaces.IPaymentQueries _paymentQueries;
        public IndexModel(IPaymentQueries paymentService)
        {
            _paymentQueries = paymentService;
        }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }

       

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {

        }
    }
}
