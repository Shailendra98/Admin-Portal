using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.ExpenseContext.Queries;
using TKW.ApplicationCore.Contexts.ExpenseContext.Services;
using TKW.ApplicationCore.Identity;



namespace TKW.AdminPortal.Areas.Expense.Pages.Ajax.Modal
{
    public class AddModel : PageModel
    {
        private IExpenseQueries _expenseQueries;
        private IAppUserService _appUser;
        private readonly IExpenseService _expenseService;
        public AddModel(IExpenseQueries expenseQueries, IAppUserService appUser, IExpenseService expenseService)
        {
            _expenseQueries = expenseQueries;
            _appUser = appUser;
            _expenseService = expenseService;
        }
        public bool IsDone { get; set; }
        public bool IsFranchise { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty]
        public decimal? Latitude { get; set; }

        [BindProperty]
        public decimal? Longitude { get; set; }

        [BindProperty]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [BindProperty]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [Display(Name = "Type")]
        [BindProperty]
        public byte? TypeId { get; set; }

        [Required(ErrorMessage = "Bill photo is required.")]
        [BindProperty]
        public IFormFile Picture { get; set; }

        public SelectList Types { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
                Types = new SelectList(await _expenseQueries.FranchiseManagerExpenseTypesAsync(cancellationToken), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                if (ModelState.IsValid)
                {
                    byte[] Picturebytes;
                    using (var ms = new MemoryStream())
                    {
                        Picture.CopyTo(ms);
                        Picturebytes = ms.ToArray();
                    }
                    var result = await _expenseService.AddFranchiseManagerExpenseAsync(
                        Amount!.Value,
                        TypeId!.Value,
                        Picturebytes,
                        Latitude,
                        Longitude,
                        Comment,
                        cancellationToken
                        );
                    if (result.IsSuccess)
                    {
                        IsDone = true;
                        return Page();
                    }
                    else
                    {
                        IsDone = false;
                        ErrorMessage = result.Error.Message;
                    }
                }
                Types = new SelectList(await _expenseQueries.FranchiseManagerExpenseTypesAsync(cancellationToken), "Id", "Name", TypeId);
            }

            return Page();
        }
    }
}
