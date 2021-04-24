using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.AdminPortal.Areas.Sell.ViewModels;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.SellContext.Errors;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.ApplicationCore.Identity;


namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal
{
    public class AddModel : PageModel
    {
        private readonly ISellQueries _sellQueries;
        private readonly IAppUserService _appUser;
        private readonly IUserQueries _userQueries;
        private readonly IMaterialQueries _materialQueries;
        private readonly IWarehouseQueries _warehouseQueries;
        private readonly ISellService _sellService;
        private readonly IBuyerQueries _buyerQueries;

        public AddModel(ISellService sellService, IBuyerQueries buyerQueries, IWarehouseQueries warehouseQueries, ISellQueries sellQueries, IAppUserService appUser, IUserQueries userQueries, IMaterialQueries materialQueries)
        {
            _sellService = sellService;
            _buyerQueries = buyerQueries;
            _warehouseQueries = warehouseQueries;
            _sellQueries = sellQueries;
            _appUser = appUser;
            _userQueries = userQueries;
            _materialQueries = materialQueries;
        }

        [BindProperty]
        public AddSellModel AddSellModel { get; set; }

        public bool IsFranchise { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var franchiseId = _appUser.Current.FranchiseId;
            IsFranchise = franchiseId.HasValue;
            if (IsFranchise)
            {
                AddSellModel = new AddSellModel();
                AddSellModel.Warehouses = new SelectList(await _warehouseQueries.WarehousesOfFranchiseAsync(franchiseId!.Value, cancellationToken), "Id", "Name");
                AddSellModel.IsGST = false;
                await UpdateBuyerSelectList();
            }
            return Page();
        }

        private async Task UpdateBuyerSelectList()
        {
            var buyers = await _buyerQueries.ActiveBuyersOfFranchiseAsync(_appUser.Current.FranchiseId!.Value);
            AddSellModel.Buyers = new SelectList(
                buyers.Select(m =>
                new
                {
                    m.Id,
                    Text = m.OwnerName + (string.IsNullOrEmpty(m.FirmName) ? "" : $" ({m.FirmName})")
                }), "Id","Text", AddSellModel.BuyerId);
            
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            //var franchiseId = _appUser.Current.FranchiseId;
            //var warehouse = new WarehouseFullDOM();
            //if (addSellModel.WarehouseId.HasValue)
            //{
            //    warehouse = await _warehouseService.GetWarehouseByIdAsync(addSellModel.WarehouseId.Value, cancellationToken);
            //    if (warehouse == null || (_appUser.Current.Role != Role.Enum.Admin && warehouse.Franchise.Id != franchiseId))
            //        ModelState.AddModelError(nameof(addSellModel.WarehouseId), "Warehouse is invalid.");
            //}
            //if (addSellModel.BuyerId.HasValue)
            //{
            //    addSellModel.Buyer = await _sellQueries.GetBuyerByIdAsync(addSellModel.BuyerId.Value, cancellationToken);
            //    if (addSellModel.Buyer == null)
            //        ModelState.AddModelError(nameof(addSellModel.BuyerId), "Buyer is invalid.");
            //}
            var franchiseId = _appUser.Current.FranchiseId;
            IsFranchise = franchiseId.HasValue;
            if (!IsFranchise) return Page();
            if (ModelState.IsValid)
            {
                var result = await _sellService.AddHandledSell(AddSellModel.WarehouseId!.Value,
                        AddSellModel.BuyerId!.Value,
                        AddSellModel.Materials.Select(m => (m.Id!.Value, m.Rate!.Value, m.Quantity!.Value, m.DifferenceQuantity!.Value)).ToList(),
                         AddSellModel.HandlerIds,
                         AddSellModel.HandledTime!.Value,
                         AddSellModel.IsGST,
                         AddSellModel.IsIGST
                        );
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else ErrorMessage = result.Error.Message;      
            }
            AddSellModel.Warehouses = new SelectList(await _warehouseQueries.WarehousesOfFranchiseAsync(franchiseId!.Value, cancellationToken), "Id", "Name", AddSellModel.WarehouseId);
            await UpdateBuyerSelectList();
            if (AddSellModel.HandlerIds != null && AddSellModel.HandlerIds.Count > 0)
                AddSellModel.Handlers = await _userQueries.UsersByIdsAsync(AddSellModel.HandlerIds, cancellationToken);
            
            if (AddSellModel.Materials != null && AddSellModel.Materials.Count > 0)
            {
                var materials = await _materialQueries.ActiveSellMaterialsAsync(AddSellModel.Materials.Where(m => m.Id.HasValue).Select(m => m.Id!.Value), cancellationToken);
                AddSellModel.Materials.ForEach(m =>
                {
                    var material = materials.FirstOrDefault(a => a.Id == m.Id);
                    m.Name = material?.Name;
                    m.GSTPercent = material?.GSTPercent;
                });
            }
            return Page();
        }
    }
}