using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.SeedWorks;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using System.ComponentModel.DataAnnotations;
using TKW.ApplicationCore.Contexts.AreaContext.Queries;

namespace TKW.AdminPortal.Areas.Franchise.Pages
{
    public class PincodesModel : PageModel
    {
        private readonly IFranchiseQueries _franchiseQueries;
        private readonly IEmployeeQueries _employeeQueries;
        private IPincodeService _pincodeService;
        private readonly IAreaQueries _areaQueries;

        public PincodesModel(IFranchiseQueries franchiseQueries, IPincodeService pincodeService, IEmployeeQueries employeeQueries, IAreaQueries areaQueries)
        {
            _employeeQueries = employeeQueries;
            _franchiseQueries = franchiseQueries;
            _pincodeService = pincodeService;
            _areaQueries = areaQueries;
        }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        public FranchiseModel FranchiseDetail { get; set; }
        public List<PincodeModel> PincodeList { get; set; }

        public List<ApplicationCore.Contexts.AreaContext.DTOs.LocalityModel> Localities { get; set; }

   
        [BindProperty]
        public List<AddPincodeModel> AddPincode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Manager is required.")]
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        public SelectList? Managers { get; set; }


        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; } 
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseDetail = await _franchiseQueries.FranchiseDetailbyIdAsync(Id,cancellationToken);
            PincodeList = await _franchiseQueries.PincodesListOfFranchiseAsync(Id,cancellationToken);
            Localities = await _areaQueries.LocalitiesByFranchiseIdAsync(Id, cancellationToken);
            ManagerId = PincodeList.FirstOrDefault()?.ManagerId;
            Managers = new SelectList(await _employeeQueries.EmployeesOfFranchiseAsync(Id, new List<int> { UserRole.PickupManager.Id, UserRole.FranchiseAdmin.Id }, new List<int> { EmployeeStatus.Active.Id }, cancellationToken), "Id", "Name", ManagerId);
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _pincodeService.UpdatePincodesOfFranchiseAsync(Id, AddPincode.Select(m => (m.Pincode, ManagerId!.Value, new WorkingDays(m.Mon, m.Tue, m.Wed, m.Thu, m.Fri, m.Sat, m.Sun))).ToList(), cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                     
                    return Content("<div class='alert alert-danger alert-dismissible fade show' role='alert'> Pincodes has been updated!<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
                }
                ErrorMessage = result.Error.Message;
            }
            else{ 
                ErrorMessage = string.Join("; " ,ModelState.Values.SelectMany(m => m.Errors).Select(m => m.ErrorMessage));
            }
            return Content("<div class='alert alert-danger alert-dismissible fade show' role='alert'>" + ErrorMessage + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button></div>");
        }
    } 
}
//< div class= "alert alert-warning alert-dismissible fade show" role = "alert" >

//  Pincodes has been updated!  < strong > Holy guacamole! </ strong > You should check in on some of those fields below.
//  <button type="button" class= "btn-close" data - bs - dismiss = "alert" aria - label = "Close" ></ button >
//       </ div >