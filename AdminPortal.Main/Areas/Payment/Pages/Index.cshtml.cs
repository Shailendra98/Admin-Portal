using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IFranchiseQueries _franchiseQueries;


        public IndexModel(IPaymentQueries paymentService, IAppUserService appUser, IFranchiseQueries franchiseQueries)
        {
            _paymentQueries = paymentService;
            _appUser = appUser;
            _franchiseQueries = franchiseQueries;

        }
        public bool FranchiseMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }
        public MultiSelectList Franchises { get; set; }


        public List<PaymentTransactionStatus> Statuses { get; set; }
        public List<PaymentMethod> Methods { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;

            if (!FranchiseMode) 
            {
                Franchises = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name"/*, Filter?.Franchises*/);
            }
            Statuses = Enumeration.GetAll<PaymentTransactionStatus>().ToList();
            Methods = Enumeration.GetAll<PaymentMethod>().ToList();
        }
    }
}
