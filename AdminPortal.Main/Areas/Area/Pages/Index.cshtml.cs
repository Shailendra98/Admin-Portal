using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.Contexts.AreaContext.DTOs;
using TKW.ApplicationCore.Contexts.AreaContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Area.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IAreaQueries _areaQueries;

        public IndexModel(IAppUserService appUser, IAreaQueries areaQueries)
        {
            _appUser = appUser;
            _areaQueries = areaQueries;
        }
        public bool IsFranchise { get; set; }
        public List<CityWithStateModel> Cities { get; set; }
        public List<int> OperatingCities { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId.HasValue;
            Cities = await _areaQueries.AllCitiesAsync(cancellationToken);

           OperatingCities = await _areaQueries.OperatingCityIdsAsync(cancellationToken);
        }
    }
}
