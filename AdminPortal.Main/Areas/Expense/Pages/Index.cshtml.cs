using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.ExpenseContext.Aggregates;
using TKW.ApplicationCore.Contexts.ExpenseContext.DTOs;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Expense.Pages
{
    public class IndexModel : PageModel
    {
        private IExpenseQueries _expenseQueries;
        private IAppUserService _appUser;
        private IFranchiseQueries _franchiseQueries;
      
        public IndexModel(IExpenseQueries expenseService, IAppUserService appUser, IFranchiseQueries franchiseQueries)
        {
            _expenseQueries = expenseService;
            _appUser = appUser;
            _franchiseQueries = franchiseQueries;
           
        }
        public bool FranchiseMode { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public ExpenseFilterModel Filter { get; set; }
        public MultiSelectList Franchises { get; set; }
        
        public MultiSelectList Types { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;
            if (!FranchiseMode)
                Franchises = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", Filter?.Franchises);

            Types = new MultiSelectList( await _expenseQueries.FranchiseManagerExpenseTypesAsync(cancellationToken), "Id", "Name", Filter?.Types);
        }

    }
}
