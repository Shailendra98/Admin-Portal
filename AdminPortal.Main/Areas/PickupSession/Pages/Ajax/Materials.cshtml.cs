using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Purchase;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.PickupSession.Pages.Ajax
{
    public class MaterialsModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IRequestQueries _requestQueries;
        public MaterialsModel(IRequestQueries requestQueries, IAppUserService appUser)
        {
            _requestQueries = requestQueries;
            _appUser = appUser;
        }
        public bool IsFranchise { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
         
        public List<PurchaseMaterialDetailsOfPickupSessionModel> PurchaseMaterials { get; set; }
        
        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            PurchaseMaterials = await _requestQueries.MaterialDetailsOfPickupSessionAsync(Id, cancellationToken);
        }
    }
}
