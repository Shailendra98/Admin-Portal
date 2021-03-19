using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;
using TKW.ApplicationCore.Contexts.ExpenseContext.DTOs;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    public class ExpensesModel : PageModel
    {
        private readonly IExpenseQueries _expenseQueries;
        private readonly IAppUserService _appUser;

        public ExpensesModel(IExpenseQueries expenseQueries, IAppUserService appUser)
        {
            _expenseQueries = expenseQueries;
            _appUser = appUser;
        }
        public bool IsFranchise { get; set; }
        public int id { get; set; }
        public List<ExpenseModel> Expenses { get; set; }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId != null;
            if (IsFranchise)
            {
                Expenses = await _expenseQueries.ExpensesOfPickupSessionAsync(id,cancellationToken);
            }
        }
    }
}
