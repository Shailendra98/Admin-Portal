using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    public class SellModel : PageModel
    {
        private readonly ISellQueries _sellQueries;
        public SellModel(ISellQueries sellQueries)
        {
            _sellQueries = sellQueries;
           
        }
        public bool IsFranchise { get; set; }
        public List<Queries.DTOs.Sell.SellModel> Sells { get; set; }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            Sells = await _sellQueries.SellsOfPickupSessionAsync(id, null, cancellationToken);
        }
    }
}
