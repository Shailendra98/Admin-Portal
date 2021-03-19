using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.Queries;

namespace TKW.AdminPortal.Areas.Sell.Pages
{
    public class DetailsModel : PageModel
    {
        private ISellQueries _sellQueries;

        public DetailsModel(ISellQueries sellQueries)
        {
            _sellQueries = sellQueries;
        }

        public SellModel Sell { get; set; }

        public async Task OnGetAsync(int Id, CancellationToken cancellationToken)
        {
          Sell = await _sellQueries.SellByIdAsync(Id, cancellationToken);
        }
    }
}