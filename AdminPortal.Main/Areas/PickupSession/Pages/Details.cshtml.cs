using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;
using System.ComponentModel.DataAnnotations;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.PickupSession;
using TKW.ApplicationCore.Contexts.PickupSessionContext.Services;

namespace TKW.AdminPortal.Areas.PickupSession.Pages
{
    public class DetailsModel : PageModel
    {

        private readonly  IPickupSessionQueries _pickupSessionQueries;

        public DetailsModel(IAppUserService appUser, IPickupSessionQueries pickupSessionQueries, IRequestQueries requestQueries,IPickupSessionService pickupSessionService)
        {
            _pickupSessionQueries = pickupSessionQueries;
            
        }
       
        [BindProperty(SupportsGet = true)]
        public string? Tab {  get; set; }
        public PickupSessionModel? PickupSession { get; set; }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken)
        {
            PickupSession = await _pickupSessionQueries.PickupSessionAsync(id,cancellationToken);
        }
    }
}
