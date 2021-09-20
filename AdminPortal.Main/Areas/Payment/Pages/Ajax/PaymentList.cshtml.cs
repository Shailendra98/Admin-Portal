using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Identity;
using TKW.Queries.DTOs.Payment;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.Payment.Pages.Ajax
{
    public class PaymentListModel : PageModel
    {
        private IPaymentQueries _paymentQueries;
        private IAppUserService _appUser;

        public PaymentListModel(IPaymentQueries paymentQueries, IAppUserService appUser)
        {
            _paymentQueries = paymentQueries;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }

        public PagedList<PaymentTransactionListItemModel> Payments { get; set; }

        public async Task OnGetAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)

                Payments = await _paymentQueries.FilteredPaymentTransactionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)

                Payments = await _paymentQueries.FilteredPaymentTransactionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
    }
}
