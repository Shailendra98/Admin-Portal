using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Employee.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax
{
    public class ListModel : PageModel
    {
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IAppUserService _appUser;

        public ListModel(IEmployeeQueries employeeService,IAppUserService appUser)
        {
            _employeeQueries = employeeService;
            _appUser = appUser;
        }
        public IEnumerable<EmployeeViewModel> List { get; set; }

        public async Task OnGetAsync(int? RoleId,CancellationToken cancellationToken)
        {
            if (_appUser.Current.FranchiseId.HasValue)
            {
                if(RoleId == UserRole.PickupBoy.Id)
                {
                    List = (await _employeeQueries.PickupBoysOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken))
                        .Select(m => new EmployeeViewModel
                        {
                            Id = m.Id,
                            ManagerName = m.Manager?.Name,
                            Name = m.Name,
                            MobileNo = m.MobileNo,
                            PictureName = m.PictureName,
                            Role = m.Role,
                            Status = m.Status
                        });
                }else if(RoleId == UserRole.FranchiseAdmin.Id || RoleId == UserRole.PickupManager.Id || RoleId == UserRole.WarehouseManager.Id)
                {
                    List = (await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId.Value, new List<int> { RoleId.Value }, new List<int> { EmployeeStatus.Active.Id, EmployeeStatus.Pending.Id, EmployeeStatus.Leave.Id }))
                        .Select(m=>new EmployeeViewModel
                        {
                            Id = m.Id,
                            Name = m.Name,
                            MobileNo = m.MobileNo,
                            PictureName = m.PictureName,
                            Role = m.Role,
                            Status = m.Status
                        });
                }else
                {
                    List = (await _employeeQueries.EmployeesOfFranchiseAsync(_appUser.Current.FranchiseId.Value, null , new List<int> { EmployeeStatus.Left.Id }))
                    .Select(m => new EmployeeViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        MobileNo = m.MobileNo,
                        PictureName = m.PictureName,
                        Role = m.Role,
                        Status = m.Status
                    });

                }
            }
        }
    }
}
