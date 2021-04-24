using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Utils;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.FranchiseContext.Services;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal
{
                                                                        
    public class ChangeStatusModel : PageModel
    {

        private readonly IEmployeeQueries _employeeQueries;
        private readonly IEmployeeService _employeeService;
        public ChangeStatusModel(IEmployeeQueries employeeQueries, IEmployeeService employeeService)
        {
            _employeeQueries = employeeQueries;
            _employeeService = employeeService;
        }

        [BindProperty]
        [Required(ErrorMessage="Status is required.")]
        [Display(Name ="Status")]
        public int? StatusId { get; set; }

        [BindProperty]
        public string? Comment { get; set; }

        [BindProperty(SupportsGet = true)]
        [FromRoute]
        public int Id { get; set; }

        public bool IsDone { get; set; }
        
        public string ErrorMessage { get; set; }

        public int RoleId { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var employee = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
            if (employee == null) return Content(ModalUtils.GenerateContent("Employee Status", "<div class='alert alert-danger'>Employee does not exist.</div>","").ToString());
            StatusId = employee.StatusId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
                if (employee == null) return Content(ModalUtils.GenerateContent("Employee Status", "<div class='alert alert-danger'>Employee does not exist.</div>", "").ToString());
                if(StatusId != employee.StatusId)
                {
                    var result = await _employeeService.ChangeStatusOfEmployeeAsync(Id, StatusId!.Value, Comment, cancellationToken);
                    if (result.IsSuccess) IsDone = true;
                    else ErrorMessage = result.Error.Message;
                }
                else
                    IsDone = true;
                RoleId = employee.RoleId;
            }
            return Page();
        }
    }
}
