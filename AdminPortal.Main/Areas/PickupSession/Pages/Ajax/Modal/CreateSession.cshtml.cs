using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;
using TKW.ApplicationCore.Identity;


namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax.Modal
{
    public class CreateSessionModel : PageModel
    {
        private IAppUserService _appUser;
        private IPickupSessionService _PickupSessionService;
        private readonly IFranchiseQueries _franchiseQueries;
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IVehicleQueries _vehicleQueries;
        public CreateSessionModel(IPickupSessionService pickupSessionService, IFranchiseQueries franchiseQueries,IEmployeeQueries employeeQueries, IAppUserService appUserService, IVehicleQueries vehicleQueries) {
            _vehicleQueries = vehicleQueries;
            _appUser = appUserService;
            _PickupSessionService = pickupSessionService;
            _franchiseQueries = franchiseQueries;
            _employeeQueries = employeeQueries;
        }
        public bool IsFranchise { get; set; }
        public bool HasFreeVehicles { get; set; }
        public bool HasFreePickupBoys { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        [Display(Name = "Pickup boys")]
        [BindProperty]
        [Required(ErrorMessage = "At least one pickup boy is required.")]
        public List<int> PickupBoyIds { get; set; }
        public MultiSelectList PickupBoys { get; set; }

        [Display(Name = "Vehicle")]
        [BindProperty]
        [Required(ErrorMessage = "Vehicle is required.")]
        public int? VehicleId { get; set; }
        public SelectList Vehicles { get; set; }

        [Display(Name = "Manager")]
        [BindProperty]
        [Required(ErrorMessage = "Manager is required.")]
        public int? ManagerId { get; set; }
        public SelectList Managers { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Cash")]
        public decimal Cash { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Odometer Reading")]
        public int OdometerReading { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                var vehicleList = await _vehicleQueries.VehiclesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                Vehicles = new SelectList(vehicleList.Select(m => new { m.Id, Text = m.Name + " (" + m.Number + ")" }), "Id", "Text");
                var pickupBoyList = await _employeeQueries.FreeActivePickupEmployeeOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                PickupBoys = new SelectList(pickupBoyList, "Id", "Name");
                Managers = new SelectList(await _employeeQueries.ActivePickupManagersAndFranchiseAdminsAsync(cancellationToken), "Id", "Name");
                HasFreePickupBoys = pickupBoyList != null && pickupBoyList.Count > 0;
                HasFreeVehicles = vehicleList != null && vehicleList.Count > 0;
            }
            IsDone = false;
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _PickupSessionService.CreatePickupSessionAsync(ManagerId!.Value, VehicleId!.Value, Cash ,OdometerReading, PickupBoyIds, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                var vehicleList = await _vehicleQueries.VehiclesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                Vehicles = new SelectList(vehicleList.Select(m => new { m.Id, Text = m.Name + " (" + m.Number + ")" }), "Id", "Text", VehicleId);
                var pickupBoyList = await _employeeQueries.FreeActivePickupEmployeeOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken);
                PickupBoys = new MultiSelectList(await _employeeQueries.FreeActivePickupEmployeeOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken), "Id", "Name", PickupBoyIds);
                Managers = new SelectList(await _employeeQueries.ActivePickupManagersAndFranchiseAdminsAsync(cancellationToken), "Id", "Name", ManagerId);
                HasFreePickupBoys = pickupBoyList != null && pickupBoyList.Count > 0;
                HasFreeVehicles = vehicleList != null && vehicleList.Count > 0;
            }
            return Page();
        }
    }
}
