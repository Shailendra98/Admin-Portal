using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Employee.ViewModels;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Constants;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AccountContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal.Add
{
    public class AddModel : PageModel
    {

        private readonly IAppUserService _appUser;
        private readonly IUserQueries _userQueries;
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IEmployeeService _employeeService;
        
        public AddModel(IAppUserService appUserService, IUserQueries userQueries, IEmployeeQueries employeeQueries, IEmployeeService employeeService)
        {
            _appUser = appUserService;
            _userQueries = userQueries;
            _employeeQueries = employeeQueries;
            _employeeService = employeeService;
        }

        public bool IsDone { get; set; }
       
        public string ErrorMessage { get; set; }
        
        [BindProperty]
        public EmployeeInputViewModel InputModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string mobileNo, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(mobileNo) || !Regex.IsMatch(mobileNo, RegexPatterns.MobileNo)) return RedirectToPage("MobileNo");
            InputModel = new EmployeeInputViewModel
            {
                MobileNo = mobileNo,
                MobileNoDisabled = true,
            };
            var user = await _userQueries.UserByMobileNumberAsync(mobileNo, cancellationToken);
            if (user != null)
            {
                InputModel.Name = user.Name;
                InputModel.NameDisabled = true;
                var employee = await _employeeQueries.EmployeeAsync(user.Id, cancellationToken);
                if(employee!=null && employee.Status != EmployeeStatus.Left)
                    return Content(ModalUtils.GenerateContent("Add Employee", "<div class='alert alert-danger'>Employee with this mobile number is already registered.</div>", "").ToString());
            }
            InputModel.PickupManagers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if(InputModel.RoleId == UserRole.PickupBoy.Id)
                {
                    var employee = await _employeeService.AddPickupBoyAsync(InputModel.MobileNo, InputModel.Name, _appUser.Current.FranchiseId!.Value, InputModel.ManagerId!.Value, false, cancellationToken);
                    if (employee.IsSuccess)
                        IsDone = true;
                    else
                        ErrorMessage = employee.Error.Message;
                }
                else
                {
                    var employee = await _employeeService.AddEmployeeAsync(InputModel.MobileNo, InputModel.Name, _appUser.Current.FranchiseId!.Value, InputModel.RoleId!.Value, cancellationToken);

                    if (employee.IsSuccess)
                        IsDone = true;
                    else
                        ErrorMessage = employee.Error.Message;

                }
            }
            if (!IsDone || !ModelState.IsValid) 
            {
                InputModel.MobileNoDisabled = true;
                var user = await _userQueries.UserByMobileNumberAsync(InputModel.MobileNo, cancellationToken);
                if (user != null)
                {
                    InputModel.Name = user.Name;
                    InputModel.NameDisabled = true;
                }
                InputModel.PickupManagers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", InputModel.ManagerId);
            }
            return Page();
        }
    }
}
