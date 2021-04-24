using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.Employee.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.ApplicationCore.Contexts.FranchiseContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.FranchiseContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal.Add
{
    public class EditModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IEmployeeService _employeeService;
        public EditModel(IEmployeeQueries employeeQueries, IEmployeeService employeeService, IAppUserService appUserService)
        {
            _employeeQueries = employeeQueries;
            _employeeService = employeeService;
            _appUser = appUserService;
        }

        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public EmployeeInputViewModel InputModel { get; set; }

        [BindProperty(SupportsGet = true)]
        [FromRoute]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var employee = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
            if (employee == null) return Content(Utils.ModalUtils.GenerateContent("Edit Employee", "<div class='alert alert-danger'>Employee does not exist any longer.</div>", "").ToString());

            InputModel = new EmployeeInputViewModel
            {
                MobileNo = employee.MobileNo,
                Name = employee.Name,
                RoleId = employee.Role.Id,
                MobileNoDisabled = false,
                NameDisabled = false
            };

            if (employee.Role == UserRole.PickupBoy)
                InputModel.ManagerId = (await _employeeQueries.ManagerOfPickupBoyAsync(Id, cancellationToken))?.Id;

            InputModel.PickupManagers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {

                var result = await _employeeService.EditEmployeeAsync(Id, InputModel.MobileNo, InputModel.Name, InputModel.RoleId!.Value, cancellationToken);
                if (result.IsSuccess)
                {
                    if (InputModel.RoleId == UserRole.PickupBoy.Id)
                    {
                        if (InputModel.ManagerId.HasValue)
                        {
                            var pickupBoyResult = await _employeeService.UpdatePickupBoyAsync(Id, InputModel.ManagerId.Value, false, cancellationToken);
                            if (pickupBoyResult.IsSuccess) IsDone = true;
                            else if (pickupBoyResult.Error is ManagerNotFoundError) ModelState.AddModelError(nameof(InputModel.ManagerId), pickupBoyResult.Error.Message);
                            else ErrorMessage = pickupBoyResult.Error;
                        }
                        else
                            ModelState.AddModelError(nameof(InputModel.ManagerId), "Please select a manager.");
                    }
                    else
                        IsDone = true;
                }
                else
                    ErrorMessage = result.Error.Message;
            }
            if (!IsDone || !ModelState.IsValid)
                InputModel.PickupManagers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", InputModel.ManagerId);

            return Page();
        }
    }
}
