using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.ExpenseContext.DTOs;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;

namespace TKW.AdminPortal.Areas.Expense.Pages.Ajax.Modal
{
    public class DetailsModel : PageModel
    {
        private IExpenseQueries _expenseQueries;

        public DetailsModel(IExpenseQueries expenseQueries)
        {
            _expenseQueries = expenseQueries;
        }

        public ExpenseModel Expense { get; set; }

        public async Task<IActionResult> OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var expense = await _expenseQueries.ExpenseByIdAsync(Id, cancellationToken);
            if (expense == null) return Content(ModalUtils.GenerateContent("Expense Details","<div class='alert alert-danger'>Expense does not exist.</div>","").ToString());
            Expense = expense;
            return Page();
        }
    }
}
