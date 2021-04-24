using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using System.Threading;
using TKW.ApplicationCore.Identity;
using TKW.AdminPortal.Areas.Material.ViewModels;
using TKW.Queries.DTOs.Material;
using TKW.ApplicationCore.Contexts.MaterialContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Material.Pages.Ajax.Modal
{
    public class AddUnsegregatedMaterialModel : PageModel
    {
        private readonly IMaterialQueries _materialQueries;
        private IMaterialService _materialService;
        private readonly IAppUserService _appUser;

        public AddUnsegregatedMaterialModel(IMaterialQueries materialQueries, IAppUserService appUser, IMaterialService materialService)
        {
            _materialQueries = materialQueries;
            _materialService = materialService;
            _appUser = appUser;
        }

        [BindProperty]
        [Required(ErrorMessage = "Material name is required.")]
        [Display(Name = "Material Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unit is required.")]
        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

        public List<MaterialUnit> Units { get; set; }

        [BindProperty]
        [Display(Name = "Segregated Materials")]
        [Required(ErrorMessage = "At least one segregated material is required.")]
        public List<int> SegregatedMaterialIds { get; set; }
        public MultiSelectList? SegregatedMaterials { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
                SegregatedMaterials = new MultiSelectList(await _materialQueries.ActiveSellMaterialsAsync(cancellationToken), "Id", "Name");
                UnitId = MaterialUnit.Kg.Id;
                Units = Enumeration.GetAll<MaterialUnit>().ToList();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _materialService.AddPurchaseStockMaterialAsync(Name, UnitId!.Value, SegregatedMaterialIds, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            SegregatedMaterials = new MultiSelectList(await _materialQueries.ActiveSellMaterialsAsync(cancellationToken), "Id", "Name",SegregatedMaterialIds);
            Units= Enumeration.GetAll<MaterialUnit>().ToList();
            return Page();
        }
    }
}
