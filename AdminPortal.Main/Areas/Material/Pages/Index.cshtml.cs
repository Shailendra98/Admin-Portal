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
using TKW.ApplicationCore.Contexts.MaterialContext.DTOs;
using TKW.ApplicationCore.Contexts.MaterialContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Material.Pages
{
    public class IndexModel : PageModel
    {
        private IAppUserService _appUser;
        private IMaterialQueries _materialQueries;

        public IndexModel(IMaterialQueries materialQueries, IAppUserService appUser)
        {
            _materialQueries = materialQueries;
            _appUser = appUser;
        }

        public List<PurchaseMaterialFullModel> PurchaseMaterials { get; set; }
        public List<PurchaseStockMaterialFullModel> PurchaseStockMaterials { get; set; }
        public List<SellMaterialFullModel> SegregatedMaterials { get; set; }

        public bool IsSuperAdmin { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            IsSuperAdmin = _appUser.Current.Role == ApplicationCore.Enums.UserRole.Admin;
            PurchaseMaterials = await _materialQueries.PurchaseMaterialListAsync(cancellationToken);
            PurchaseStockMaterials = await _materialQueries.PurchaseStockMaterialListAsync(cancellationToken);
            SegregatedMaterials = await _materialQueries.SellMaterialListAsync(cancellationToken);
        }
    }
}
