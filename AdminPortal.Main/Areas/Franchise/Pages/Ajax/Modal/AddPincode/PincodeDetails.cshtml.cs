using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.Queries.DTOs.Franchise;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax.Modal
{
    public class PincodeDetailsModel : PageModel
    {
        private readonly IEmployeeQueries _employeeQueries;

        private readonly IAreaQueries _areaQueries;

        private readonly IPincodeService _pincodeService;

        [BindProperty]
        public PincodeModel PincodeModel { get; set; }

        public PincodeDetailsModel(IEmployeeQueries employeeQueries, IAreaQueries areaQueries, IPincodeService pincodeService)
        {
            _employeeQueries = employeeQueries;
            _areaQueries = areaQueries;
            _pincodeService = pincodeService;

        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PincodeNo { get; set; }


        public string ErrorMessage { get; set; }

        public List<Queries.DTOs.Area.LocalityModel> Localities { get; set; }

        public SelectList? Managers { get; set; }

        public bool IsDone { get; set; }


        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            PincodeModel = new PincodeModel()
            {
                Pincode = PincodeNo,
            };

            Managers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(Id, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", PincodeModel.ManagerId);
            Localities = await _areaQueries.LocalitiesByPincodeAsync(PincodeNo, cancellationToken);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var newpincodeNumber = await _pincodeService.UpdatePincodeOfFranchiseAsync(Id, PincodeModel.Pincode, PincodeModel.ManagerId, new WorkingDays(PincodeModel.Mon, PincodeModel.Tue, PincodeModel.Wed, PincodeModel.Thu, PincodeModel.Fri, PincodeModel.Sat, PincodeModel.Sun), cancellationToken);
                if (newpincodeNumber.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = newpincodeNumber.Error.Message;
            }
            Managers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(Id, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", PincodeModel.ManagerId);
            Localities = await _areaQueries.LocalitiesByPincodeAsync(PincodeNo, cancellationToken);
            return Page();
        }
    }
}
