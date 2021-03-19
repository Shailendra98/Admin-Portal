using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using System.Threading;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.MaterialContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Material.Pages.Ajax.Modal
{
    public class AddSegregatedMaterialModel : PageModel
    {
        private readonly IMaterialQueries _materialQueries;
        private readonly IAppUserService _appUser;
        private readonly IMaterialService _materialService;

        public AddSegregatedMaterialModel(IMaterialQueries materialQueries, IAppUserService appUser,IMaterialService materialService)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
            _materialService = materialService;
        }


        [BindProperty]
        [Required(ErrorMessage = "Material name is required.")]
        [Display(Name = "Material Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unit is required.")]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        public List<MaterialUnit> Units { get; set; }

        [BindProperty]
        [Display(Name = "Unsegregated Materials")]
        [Required(ErrorMessage = "At least one segregated material is required.")]
        public List<int> UnsegregatedMaterialIds { get; set; }
        public MultiSelectList? UnsegregatedMaterials { get; set; }

        [BindProperty]
        [Display(Name = "GST Percent")]
        public decimal? GSTPercent { get; set; }

        [BindProperty]
        [Display(Name = "HSN Code")]
        public int? HSNCode { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
               UnsegregatedMaterials = new MultiSelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), "Id", "Name");
               UnitId = MaterialUnit.Kg.Id;
               Units = Enumeration.GetAll<MaterialUnit>().ToList();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _materialService.AddSellMaterialAsync(Name, UnitId, UnsegregatedMaterialIds, GSTPercent, HSNCode, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            UnsegregatedMaterials = new MultiSelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), "Id", "Name",UnsegregatedMaterialIds);
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            return Page();
        }
    }
}
