using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.ApplicationCore.Contexts.AreaContext.Queries;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;

namespace TKW.AdminPortal.Areas.Franchise.Pages.Ajax
{
    public class PincodeDaysModel : PageModel
    {
        private readonly IFranchiseQueries _franchiseQueries;
        private IPincodeService _pincodeService;
        private readonly IAreaQueries _areaQueries;
        public PincodeDaysModel(IFranchiseQueries franchiseQueries,IPincodeService pincodeService,IAreaQueries areaQueries)
        {
            _franchiseQueries = franchiseQueries;
            _pincodeService = pincodeService;
            _areaQueries = areaQueries;
        }

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        [BindProperty]
        public List<AddPincodeModel> AddPincode { get; set; }

        public List<PincodeModel> PincodeList { get; set; }

        public List<ApplicationCore.Contexts.AreaContext.DTOs.LocalityModel> Localities { get; set; }


        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            PincodeList = await _franchiseQueries.PincodesListOfFranchiseAsync(Id, cancellationToken);
            Localities = await _areaQueries.LocalitiesByFranchiseIdAsync(Id, cancellationToken);
        }
    }
}
