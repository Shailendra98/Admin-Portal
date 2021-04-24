using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.FranchiseContext.Services;
using TKW.Queries.DTOs.Franchise;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax
{
    public class ListModel : PageModel
    {
        
        private readonly IFranchiseQueries _franchiseQueries;

        public ListModel(IFranchiseQueries franchiseQueries)
        {
            _franchiseQueries = franchiseQueries;
        }

        public List<FranchiseModel> Franchises { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Franchises = await _franchiseQueries.AllFranchisesAsync(cancellationToken);
        }
    }
}
