using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.Account.Services;
using TKW.Employee.Exceptions;
using TKW.Request.DTOs.Input;
using TKW.Request.Exceptions;
using TKW.Request.Services;
using TKW.Warehouse.Exceptions;
using TKW.Warehouse.Services;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax.Modal
{
    public class StoreModel : PageModel
    {
        private IWarehouseService _warehouseService;
        private IAccountService _accountService;
        private IRequestService _requestService;

        public StoreModel(IRequestService requestService,IWarehouseService warehouseService,IAccountService accountService)
        {
            _requestService = requestService; 
            _warehouseService = warehouseService;
            _accountService = accountService;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public List<long> Ids { get; set; }

        [Required]
        [BindProperty]
        public ViewModels.StoreModel Model { get; set; }
        
        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Model = new ViewModels.StoreModel
            {
                Warehouses = new SelectList(await _warehouseService.GetAllActiveWarehousesAsync(cancellationToken), "Id", "Name")
            };
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _requestService.StoreRequestMaterialsAsync(new StoreRequestMaterialsDIM {
                        RequestIds = Ids,
                        StorerId = Model.StorerId.Value,
                        StoreTime = Model.StoreTime.Value,
                        WarehouseId = Model.WarehouseId.Value
                    });
                    IsDone = true;
                }
                catch (RequestNotFoundException)
                {
                    ErrorMessage = "One or more request does not exist.";
                }
                catch (EmployeeNotFoundException)
                {
                    ErrorMessage = "Storer does not exist or is invalid.";
                }
                catch (WarehouseNotFoundException)
                {
                    ErrorMessage = "Warehouse is invalid or does not exist.";
                }
            }
            if (Model.StorerId.HasValue)
                Model.Storer = await _accountService.GetUserByUserIdAsync(Model.StorerId.Value, cancellationToken);
            Model.Warehouses = new SelectList(await _warehouseService.GetAllActiveWarehousesAsync(cancellationToken), "Id", "Name",Model.WarehouseId);
            return Page();
        }
    }
}