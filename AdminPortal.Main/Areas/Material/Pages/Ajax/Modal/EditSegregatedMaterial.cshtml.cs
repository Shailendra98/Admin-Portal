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
    public class EditSegregatedMaterialModel : PageModel
    {

        private readonly IMaterialQueries _materialQueries;
        private IMaterialService _materialService;
        private readonly IAppUserService _appUser;

        public EditSegregatedMaterialModel(IMaterialQueries materialQueries, IAppUserService appUser, IMaterialService materialService)
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
        public int UnitId { get; set; }
        public List<MaterialUnit> Units { get; set; }

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
            var material = await _materialQueries.SellMaterialbyIdAsync(Id, cancellationToken);

            IsDone = false;
            Name = material.Name;
            UnitId = material.UnitId;
            IsActive = material.IsActive;
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            GSTPercent = material.GSTPercent;
            HSNCode = material.HSNCode;
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _materialService.EditSellMaterialAsync(
                    Id,
                    Name,
                    UnitId,
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
            IsDone = false;
            Units = Enumeration.GetAll<MaterialUnit>().ToList();
            return Page();
        }
    }
}
