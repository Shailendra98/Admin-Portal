using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.AreaContext.Services;

namespace TKW.AdminPortal.Areas.Area.Pages.Ajax.Modal
{
    public class AddLocalityModel : PageModel
    {
        private ILocalityService _localityService;

        public AddLocalityModel(ILocalityService localityService)
        {
            _localityService = localityService;
        }

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
        public decimal Latitude { get; set; } = 23.2051791M;
        [BindProperty]
        public decimal Longitude { get; set; } = 77.2562647M;

        public bool IsDone { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var result = await _localityService.AddLocalityAsync(Name, Latitude, Longitude, Pincode,Id);
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
