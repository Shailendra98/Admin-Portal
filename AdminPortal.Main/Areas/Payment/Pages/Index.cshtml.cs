using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.PaymentContext.Aggregates;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;
using TKW.Queries.DTOs.Payment;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Payment.Pages
{
    public class IndexModel : PageModel
    {
        private IPaymentQueries _paymentQueries;
         public IndexModel(IPaymentQueries paymentService, IAppUserService appUser)
        {
            _paymentQueries = paymentService;
        }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }

        public List<PaymentTransactionStatus> Statuses { get; set; }
        public List<PaymentMethod> Methods { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            
            Statuses = Enumeration.GetAll<PaymentTransactionStatus>().ToList();
            Methods = Enumeration.GetAll<PaymentMethod>().ToList();
            
        }
    }
}
