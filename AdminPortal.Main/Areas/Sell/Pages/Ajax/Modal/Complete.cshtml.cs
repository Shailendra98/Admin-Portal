using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.Sell.ViewModels;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.SellContext.Services;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Sell.Pages.Ajax.Modal
{
    public class CompleteModel : PageModel
    {
        private readonly ISellQueries _sellQueries;
        private readonly IMaterialQueries _materialQueries;
        private readonly ISellService _sellService;
        private readonly IBuyerQueries _buyerQueries;

        public CompleteModel(ISellService sellService, IBuyerQueries buyerQueries, ISellQueries sellQueries, IMaterialQueries materialQueries)
        {
            _sellService = sellService;
            _buyerQueries = buyerQueries;
            _sellQueries = sellQueries;
            _materialQueries = materialQueries;
        }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty]
        public CompleteSellModel CompleteSellModel { get; set; }

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var Sell = await _sellQueries.SellByIdAsync(Id, cancellationToken);
            CompleteSellModel = new CompleteSellModel()
            {

                Materials = Sell.Items == null ? null : Sell.Items.Select(m => new SellMaterialRateQuantityInputModel
                {
                    Id = m.MaterialId,
                    Name = m.MaterialName,
                    Quantity = m.ActualQuantity
                }).ToList(),

            };

            CompleteSellModel.IsGST = false;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {

            if (ModelState.IsValid)
            {
                var result = await _sellService.CompleteSellAsync(Id,
                        CompleteSellModel.Materials.Select(m => (m.Id!.Value, m.Rate!.Value, m.Quantity!.Value, m.DifferenceQuantity!.Value)).ToList(),
                         CompleteSellModel.IsGST,
                         CompleteSellModel.IsIGST
                        );
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                else ErrorMessage = result.Error.Message;
            }
            var Sell = await _sellQueries.SellByIdAsync(Id, cancellationToken);
            CompleteSellModel = new CompleteSellModel()
            {
                Materials = Sell.Items == null ? null : Sell.Items.Select(m => new SellMaterialRateQuantityInputModel
                {
                    Id = m.MaterialId,
                    Name = m.MaterialName,
                    Quantity = m.ActualQuantity
                }).ToList(),

            };
            return Page();
        }
    }
}
