using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Contexts.MaterialContext.Services;
using TKW.ApplicationCore.Contexts.MaterialContext.DTOs;
using TKW.ApplicationCore.Contexts.MaterialContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.Areas.Material.Pages.Ajax.Modal
{
    public class EditPurchaseMaterialModel : PageModel
    {
        private readonly IMaterialQueries _materialQueries;
        private IMaterialService _materialService;
        private readonly IAppUserService _appUser;

        public EditPurchaseMaterialModel(IMaterialQueries materialQueries, IAppUserService appUser, IMaterialService materialService)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
            _materialService = materialService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Material name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unit is required.")]
        [Display(Name = "Unit")]
        public int? UnitId { get; set; }
        public List<MaterialUnit> Units { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Unsegregated material is required.")]
        [Display(Name = "Unsegregated Stock Material")]
        public int? UnsegregatedMaterialId { get; set; }

        public SelectList UnsegregatedMaterial { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Material type is required.")]
        [Display(Name = "Material Type")]
        public int? TypeId { get; set; }
        public SelectList Types { get; set; }

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

        [BindProperty]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }

        public bool IsDone { get; set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var material = await _materialQueries.PurchaseMaterialbyIdAsync(Id, cancellationToken);
            Name = material!.Name;
            UnsegregatedMaterialId = material.StockMaterial.Id;
            UnsegregatedMaterial = new SelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), nameof(PurchaseStockMaterialModel.Id), nameof(PurchaseStockMaterialModel.Name), material.StockMaterial.Id);
            TypeId = material.TypeId;
            GroupId = material.GroupId;
            Types = new SelectList(Enumeration.GetAll<MaterialType>().ToList(), "Id", "Name",material.TypeId);
            Groups = new SelectList(Enumeration.GetAll<MaterialGroup>().ToList(), "Id", "Name",material.GroupId);
            UnitId = material.UnitId;
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            GSTPercent = material.GSTPercent;
            HSNCode = material.HSNCode;
            IsActive = material.IsActive;
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _materialService.EditPurchaseMaterialAsync(
                    Id,
                    Name,
                    UnsegregatedMaterialId!.Value,
                    UnitId!.Value,
                    TypeId!.Value,
                    GroupId,
                    IsActive,
                    GSTPercent,
                    HSNCode,
                    cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            UnsegregatedMaterial = new SelectList(await _materialQueries.ActivePurchaseStockMaterialsAsync(cancellationToken), nameof(PurchaseStockMaterialModel.Id), nameof(PurchaseStockMaterialModel.Name), UnsegregatedMaterialId);
            Types = new SelectList(Enumeration.GetAll<MaterialType>().ToList(), "Id", "Name", TypeId);
            Groups = new SelectList(Enumeration.GetAll<MaterialGroup>().ToList(), "Id", "Name", GroupId);
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            return Page();
        }
    }
}
