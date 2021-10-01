using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Payment;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.User.Pages.Ajax
{
    public class UserListModel : PageModel
    {
        private IPaymentQueries _paymentQueries;

        public UserListModel(IPaymentQueries paymentQueries)
        {
            _paymentQueries = paymentQueries;
        }

        [BindProperty(SupportsGet = true)]
        public PaymentTransactionsFilterModel Filter { get; set; }

        public PagedList<PaymentTransactionListItemModel> Payments { get; set; }
        public async Task OnGetAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            Payments = await _paymentQueries.FilteredPaymentTransactionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            Payments = await _paymentQueries.FilteredPaymentTransactionsAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
    }
}
