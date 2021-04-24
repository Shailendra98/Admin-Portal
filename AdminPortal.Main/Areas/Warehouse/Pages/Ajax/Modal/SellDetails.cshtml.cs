using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Sell;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax.Modal
{
    public class SellDetailsModel : PageModel
    {
        private ISellQueries _sellQueries;

        public SellDetailsModel(ISellQueries sellQueries)
        {
            _sellQueries = sellQueries;
        }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }
        public Queries.DTOs.Sell.SellModel Sell { get; set; }
     
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Sell = await _sellQueries.SellByIdAsync(Id);
        }
    }
}
