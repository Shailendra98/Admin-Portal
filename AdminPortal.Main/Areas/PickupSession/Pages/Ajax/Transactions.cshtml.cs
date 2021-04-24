using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Identity;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.PickupSession;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    public class TransactionsModel : PageModel
    {
        private IAppUserService _appUser;
        private IPickupSessionQueries _pickupSessionQueries;

        public TransactionsModel(IAppUserService appUser, IPickupSessionQueries pickupSessionQueries)
        {
            _appUser = appUser;
            _pickupSessionQueries = pickupSessionQueries;
        }

        //[BindProperty(SupportsGet = true)]
        //[Required]
        //public int Id { get; set; }
        public bool IsFranchise { get; set; }
        public List<PickupSessionCashTransactionModel> CashTransaction { get; set; }

        public List<PickupSessionPaytmTransactionModel> Paytm { get; set; }
        
        public List<PickupSessionUPITransactionModel> Upi { get; set; }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            CashTransaction = await _pickupSessionQueries.CashTransactionsAsync(id, cancellationToken);
            Paytm = await _pickupSessionQueries.PaytmTransactionsAsync(id,cancellationToken);
            Upi = await _pickupSessionQueries.UPITransactionsAsync(id, cancellationToken);
        }
    }
}
