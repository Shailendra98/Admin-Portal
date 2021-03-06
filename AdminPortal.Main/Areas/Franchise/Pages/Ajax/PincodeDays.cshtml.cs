using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.Queries.DTOs.Franchise;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax
{
    public class PincodeDaysModel : PageModel
    {
        private readonly IFranchiseQueries _franchiseQueries;
        private IPincodeService _pincodeService;
        private readonly IAreaQueries _areaQueries;
        private readonly IEmployeeQueries _employeeQueries;
        public PincodeDaysModel(IFranchiseQueries franchiseQueries, IPincodeService pincodeService, IAreaQueries areaQueries, IEmployeeQueries employeeQueries)
        {
            _franchiseQueries = franchiseQueries;
            _pincodeService = pincodeService;
            _areaQueries = areaQueries;
            _employeeQueries = employeeQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public bool IsDone { get; set; }


        public string ErrorMessage { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Manager is required.")]
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        public SelectList? Managers { get; set; }

        public FranchiseModel FranchiseDetail { get; set; }

        [BindProperty]

        public List<PincodeModel> PincodeList { get; set; }

        public List<Queries.DTOs.Area.LocalityModel> Localities { get; set; }


        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            PincodeList = await _franchiseQueries.PincodesListOfFranchiseAsync(Id, cancellationToken);
            FranchiseDetail = await _franchiseQueries.FranchiseDetailbyIdAsync(Id, cancellationToken);
            Localities = await _areaQueries.LocalitiesByFranchiseIdAsync(Id, cancellationToken);
            ManagerId = PincodeList.FirstOrDefault()?.ManagerId;
            Managers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(Id, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", ManagerId);
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _pincodeService.UpdatePincodesOfFranchiseAsync(Id, PincodeList.Select(m => (m.Pincode, ManagerId!.Value, new WorkingDays(m.Mon, m.Tue, m.Wed, m.Thu, m.Fri, m.Sat, m.Sun))).ToList(), cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;

                    return Content("<div class='alert alert-success alert-dismissible fade show' role='alert'>Successfully updated!<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
                }
                ErrorMessage = result.Error.Message;
            }
            else
            {
                ErrorMessage = string.Join("; ", ModelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage));
            }
            return Content("<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + ErrorMessage + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
        }
    }
}
