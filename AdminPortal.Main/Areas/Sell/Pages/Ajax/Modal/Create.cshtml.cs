using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.Sell.ViewModels;
using System.ComponentModel.DataAnnotations;
using TKW.AdminPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Queries;
using TKW.ApplicationCore.Contexts.InventoryContext.Queries;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Contexts.SellContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal
{
    public class CreateModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly ISellQueries _sellQueries;
        private readonly IMaterialQueries _materialQueries;
        private readonly IWarehouseQueries _warehouseQueries;
        private readonly ISellService _sellService;
        private readonly IBuyerQueries _buyerQueries;
        private readonly IPickupSessionQueries _pickupSessionQueries;

        public CreateModel(IAppUserService appUser,ISellService sellService, IWarehouseQueries warehouseQueries,IBuyerQueries buyerQueries, ISellQueries sellQueries, IMaterialQueries materialQueries,IPickupSessionQueries pickupSessionQueries)
        {
            _appUser = appUser;
            _warehouseQueries = warehouseQueries;
            _sellService = sellService;
            _buyerQueries = buyerQueries;
            _sellQueries = sellQueries;
            _materialQueries = materialQueries;
            _pickupSessionQueries = pickupSessionQueries;
        }

        [BindProperty]
        public CreateSellModel CreateSellModel { get; set; }

   
        public bool IsFranchise { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var franchiseId = _appUser.Current.FranchiseId;
            IsFranchise = franchiseId.HasValue;
            if (!IsFranchise) return Page();
            if (IsFranchise)
            {
                CreateSellModel = new CreateSellModel();
                CreateSellModel.Warehouses = new SelectList(await _warehouseQueries.WarehousesOfFranchiseAsync(franchiseId!.Value, cancellationToken), "Id", "Name");
                CreateSellModel.Materials = new SelectList(await _materialQueries.ActiveSellMaterialsAsync(cancellationToken), "Id", "Name");
                await UpdateBuyerSelectList();
                await UpdatePickupSessionSelectList();
            }
            return Page();
        }
        private async Task UpdateBuyerSelectList()
        {
            var buyers = await _buyerQueries.ActiveBuyersOfFranchiseAsync(_appUser.Current.FranchiseId!.Value);
            CreateSellModel.Buyers = new SelectList(
                buyers.Select(m =>
                new
                {
                    m.Id,
                    Text = m.OwnerName + (string.IsNullOrEmpty(m.FirmName) ? "" : $" ({m.FirmName})")
                }), "Id", "Text", CreateSellModel.BuyerId);

        }
        private async Task UpdatePickupSessionSelectList()
        {
            var pickupSessions = await _pickupSessionQueries.ActivePickupSessionsOfFranchiseAsync(_appUser.Current.FranchiseId!.Value);
            CreateSellModel.PickupSessions = pickupSessions.Select(m => new CreateSellModel.PickupSessionOptionModel(m)).ToList();
            //CreateSellModel.PickupSessions = new SelectList(
            //    pickupSession.Select(m =>
            //    new
            //    {
            //        m.Id,
            //        Html =  m.Vehicle.Name + " (" + m.Vehicle.Number + ")" + "<br/>" + string.Join(", ", m.PickupBoys.Where(a=>a.EndTime == null).Select(a=>a.Name))
            //    }), "Id", "Html", CreateSellModel.PickupSessionId);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _sellService.AddSellAsync(
                        CreateSellModel.BuyerId!.Value,
                        CreateSellModel.WarehouseId!.Value,
                        CreateSellModel.PickupSessionId!.Value,
                        CreateSellModel.MaterialIds.Select(m => (m,(decimal?)null)).ToList(),
                        CreateSellModel.Comment
                        );
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else ErrorMessage = result.Error.Message;
            }
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            if (IsFranchise)
            {
                CreateSellModel.Warehouses = new SelectList(await _warehouseQueries.WarehousesOfFranchiseAsync(_appUser.Current.FranchiseId!.Value, cancellationToken), "Id", "Name");
                CreateSellModel.Materials = new SelectList(await _materialQueries.ActiveSellMaterialsAsync(cancellationToken), "Id", "Name");
                await UpdateBuyerSelectList();
                await UpdatePickupSessionSelectList();
            }
            return Page();
        }
        
    }
}
