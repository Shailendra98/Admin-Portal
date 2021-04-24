using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.DTOs.Area;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Area.Pages
{
    public class PincodesModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IAreaQueries _areaQueries;

        public PincodesModel(IAppUserService appUser, IAreaQueries areaQueries)
        {
            _appUser = appUser;
            _areaQueries = areaQueries;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public CityWithLocalitiesModel City { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            City = await _areaQueries.CityWithLocalitiesAsync(Id,cancellationToken);
        }
    }
}
