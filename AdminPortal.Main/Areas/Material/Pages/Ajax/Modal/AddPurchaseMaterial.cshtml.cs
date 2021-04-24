using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.Enums;
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
    public class AddPurchaseMaterialModel : PageModel
    {

        private readonly IMaterialQueries _materialQueries;
        private IMaterialService _materialService;
        private readonly IAppUserService _appUser;

        public AddPurchaseMaterialModel(IMaterialQueries materialQueries,IAppUserService appUser,IMaterialService materialService)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
            _materialService = materialService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Material name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unit is required.")]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        public List<MaterialUnit> Units { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unsegregated material is required.")]
        [Display(Name = "Unsegregated Stock Material")]
        public int UnsegregatedMaterialId { get; set; }

        public SelectList? UnsegregatedMaterial { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "material type is required.")]
        [Display(Name = "Material Type")]
        public int TypeId { get; set; }
        public SelectList? Types { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Group is required.")]
        [Display(Name = "Material Group")]
        public int GroupId { get; set; }
        public SelectList? Groups { get; set; }

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
            UnsegregatedMaterial = new SelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), "Id", "Name");
            Types = new SelectList(Enumeration.GetAll<MaterialType>().ToList(), "Id" ,"Name");
            Groups = new SelectList(Enumeration.GetAll<MaterialGroup>().ToList(),"Id","Name");
            UnitId = MaterialUnit.Kg.Id;
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _materialService.AddPurchaseMaterialAsync(Name, UnsegregatedMaterialId, UnitId, TypeId, GroupId, GSTPercent, HSNCode, cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            UnsegregatedMaterial = new SelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), "Id", "Name",UnsegregatedMaterialId);
            Types = new SelectList(Enumeration.GetAll<MaterialType>().ToList(), "Id", "Name",TypeId);
            Groups = new SelectList(Enumeration.GetAll<MaterialGroup>().ToList(), "Id", "Name",GroupId);
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            return Page();
        }
    }
}
