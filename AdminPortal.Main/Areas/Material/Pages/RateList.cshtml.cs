using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Material.ViewModels;
using TKW.ApplicationCore.Contexts.AppContext.Queries;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Contexts.MaterialContext.Aggregates;
using TKW.ApplicationCore.Contexts.MaterialContext.DTOs;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Material.Pages
{
    public class RateListModel : PageModel
    {
        private IAppUserService _appUser;
        private IMaterialQueries _materialQueries;

        public RateListModel(IMaterialQueries materialQueries, IAppUserService appUser)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
        }

        public List<PurchaseMaterialWithRateModel> MaterialRates { get; set; }

        public List<PurchaseMaterialModel> Materials { get; set; }
        public bool IsFranchise { get; set; }

        public async Task OnGetAsync( CancellationToken cancellationToken)
        {
            IsFranchise = _appUser.Current.FranchiseId != null;
            if (IsFranchise) {
                Materials = await _materialQueries.ActivePurchaseMaterialsAsync(cancellationToken);
                MaterialRates = await _materialQueries.PurchaseMaterialRatesByFranchiseAsync(_appUser.Current.FranchiseId.Value,  cancellationToken);
            }
        }
    }
} 