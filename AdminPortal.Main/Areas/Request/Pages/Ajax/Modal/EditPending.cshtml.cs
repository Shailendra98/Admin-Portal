using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Request.ViewModels;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.PurchaseContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class EditPendingModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IRequestQueries _requestQueries;
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IRequestService _requestService;
        private readonly IMaterialQueries _materialQueries;
        public EditPendingModel(IAppUserService appUser, IMaterialQueries materialQueries, IRequestQueries requestQueries, IEmployeeQueries employeeQueries, IRequestService requestService)
        {
            _appUser = appUser;
            _materialQueries = materialQueries;
            _requestQueries = requestQueries;
            _employeeQueries = employeeQueries;
            _requestService = requestService;
        }

        [BindProperty(SupportsGet =true)]
        public long Id { get; set; }
        
        [BindProperty]
        public EditPendingRequestModel Model { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var request = await _requestQueries.RequestAsync(Id, cancellationToken);
            IsDone = false;
            Model = new EditPendingRequestModel()
            {
                Comment = request.Comment,
                Materials = request.Items == null ? null : request.Items.Select(m => new AdminPortal.ViewModels.MaterialRateQuantityInputModel
                {
                    Id = m.Material.Id,
                    Name = m.Material.Name,
                    Rate = m.Rate
                }).ToList(),
                PickupManagerId = request.PickupManagerId,
                IsFranchise = _appUser.Current.Role == ApplicationCore.Enums.UserRole.FranchiseAdmin
            };
            if (Model.IsFranchise == false)
                Model.PickupManagers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _employeeQueries.ActivePickupManagersAndFranchiseAdminsAsync(cancellationToken), "Id", "Name", request.PickupManagerId);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.UpdatePendingRequestAsync(
                    Id,
                    Model.Materials.Select(m => (m.Id.Value, m.Rate)).ToList(),
                    Model.PickupManagerId,
                    Model.Comment,
                    cancellationToken);
                if(result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            
            Model.IsFranchise = _appUser.Current.Role == ApplicationCore.Enums.UserRole.FranchiseAdmin;
            IsDone = false;
            if (Model?.Materials != null && Model.Materials.Count > 0)
            {
                var materials = await _materialQueries.ActivePurchaseMaterialsAsync(Model.Materials.Where(m => m.Id.HasValue).Select(m => m.Id.Value), cancellationToken);
                foreach (var m in Model.Materials)
                {
                    m.Name = materials.FirstOrDefault(a => a.Id == m.Id)?.Name ?? string.Empty;
                }
            }
            Model.PickupManagers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _employeeQueries.ActivePickupManagersAndFranchiseAdminsAsync(cancellationToken), "Id", "Name", Model.PickupManagerId);
            
            return Page();
        }
    }
}
