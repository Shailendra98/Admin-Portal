using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using TKW.ApplicationCore.Contexts.AreaContext.Queries;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.Infrastructure.Data.Queries;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax.Modal
{
    public class AddPincodeNumberModel : PageModel
    {
        private readonly IEmployeeQueries _employeeQueries;

        private readonly IAreaQueries _areaQueries;

        private readonly IPincodeService _pincodeService;

        [BindProperty]
        public PincodeModel PincodeModel { get; set; }

        public AddPincodeNumberModel(IEmployeeQueries employeeQueries,IAreaQueries areaQueries,IPincodeService pincodeService)
        {
            _employeeQueries = employeeQueries;
            _areaQueries = areaQueries;
            _pincodeService = pincodeService;
            PincodeModel = new PincodeModel() { };
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

      
        public string ErrorMessage { get; set; }

        public List<ApplicationCore.Contexts.AreaContext.DTOs.LocalityModel> Localities { get; set; }

        public SelectList? Managers { get; set; }

        public bool IsDone { get; set; }
     

        public async Task<IActionResult> OnGetAsync(int PincodeNo,CancellationToken cancellationToken)
        {
            PincodeModel.Pincode = PincodeNo;
            
            Managers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(Id, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name",PincodeModel.ManagerId);
            Localities = await _areaQueries.LocalitiesByPincodeAsync(PincodeNo, cancellationToken);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var newpincodeNumber = await _pincodeService.UpdatePincodeOfFranchiseAsync(Id, PincodeModel.Pincode, PincodeModel.ManagerId, new WorkingDays(PincodeModel.Mon, PincodeModel.Tue, PincodeModel.Wed, PincodeModel.Thu, PincodeModel.Fri, PincodeModel.Sat, PincodeModel.Sun),cancellationToken);
                if (newpincodeNumber.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = newpincodeNumber.Error.Message;     
            }
            return Page();
        }
    }
}
