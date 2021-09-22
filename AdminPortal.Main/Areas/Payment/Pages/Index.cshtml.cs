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
        private IAppUserService _appUser;

        public IndexModel(IPaymentQueries paymentService, IAppUserService appUser)
        {
            _paymentQueries = paymentService;
            _appUser = appUser;

        }
        public bool FranchiseMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }

        public List<PaymentTransactionStatus> Statuses { get; set; }
        public List<PaymentMethod> Methods { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;
            Statuses = Enumeration.GetAll<PaymentTransactionStatus>().ToList();
            Methods = Enumeration.GetAll<PaymentMethod>().ToList();
            if (!FranchiseMode){}
           }
    }
}
