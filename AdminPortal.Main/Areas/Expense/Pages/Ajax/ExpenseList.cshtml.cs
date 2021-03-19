using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.ExpenseContext.DTOs;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Types;
//using X.PagedList;
namespace TKW.AdminPortal.Areas.Expense.Pages.Ajax.Modal
{
    [Authorize]
    public class ExpenseListModel : PageModel
    {
        private IExpenseQueries _expenseQueries;
        private IAppUserService _appUser;

        public ExpenseListModel(IExpenseQueries expenseQueries, IAppUserService appUser)
        {
            _expenseQueries = expenseQueries;
            _appUser = appUser;
        }

        [BindProperty(SupportsGet = true)]
        public ExpenseFilterModel Filter { get; set; }
        
        public PagedList<ExpenseModel> Expenses { get; set; }
        
        public async Task OnGetAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchises = new List<int> { _appUser.Current.FranchiseId.Value };
            Expenses = await _expenseQueries.FilteredExpensesAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize, CancellationToken cancellationToken)
        {
            int size = pageSize == null ? 20 : (pageSize < 5) ? 5 : (pageSize > 200) ? 200 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.Franchises = new List<int> { _appUser.Current.FranchiseId.Value };
            Expenses = await _expenseQueries.FilteredExpensesAsync(Filter, pageNo ?? 1, size, cancellationToken);
        }
    }
}
