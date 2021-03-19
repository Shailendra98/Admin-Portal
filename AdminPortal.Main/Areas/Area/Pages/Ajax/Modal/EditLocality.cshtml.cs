using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.AreaContext.Queries;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Area.Pages.Ajax.Modal
{
    public class EditLocalityModel : PageModel
    {
        private ILocalityService _localityService;
        private readonly IAreaQueries _areaQueries;
        private readonly IAppUserService _appUser;

        public EditLocalityModel(ILocalityService localityService, IAreaQueries areaQueries,IAppUserService appUserService)
        {
            _localityService = localityService;
            _areaQueries = areaQueries;
            _appUser = appUserService;
        }

      
        [BindProperty(SupportsGet = true)]
        public int CityId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Material name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Pincode is required.")]
        [Display(Name = "Pincode")]
        public int Pincode { get; set; }

        [BindProperty]
        public decimal Latitude { get; set; }
        [BindProperty]
        public decimal Longitude { get; set; }

        [BindProperty]
        public bool IsActive { get; set; }
        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var locality = await _areaQueries.LocalityByIdAsync(Id, cancellationToken);
            Id = locality.Id;
            if(locality.Latitude.HasValue && locality.Longitude.HasValue)
            {
                Latitude = locality.Latitude.Value;
                Longitude = locality.Longitude.Value;
            }
            else
            {
                Latitude = 0;
                Longitude = 0;
            }
            Name = locality.Name;
            Pincode = locality.Pincode;
            IsActive = locality.IsActive;

        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _localityService.EditLocalityAsync(
                    Id,
                    Name, 
                    Latitude, 
                    Longitude, 
                    Pincode, 
                    CityId, 
                    IsActive,
                    cancellationToken);
                if (result.IsSuccess)
                {
                    IsDone = true;
                    return Page();
                }
                ErrorMessage = result.Error.Message;
            }
            return Page();
        }
    }
}
