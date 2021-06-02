using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Area;
using TKW.Queries.Interfaces;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Purchase;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAreaQueries _areaQueries;
        private readonly IAppUserService _appUser;
        private readonly IFranchiseQueries _franchiseQueries;
        private readonly IRequestQueries _requestQueries;
        private readonly IUserQueries _userQueries;
        public IndexModel(IFranchiseQueries franchiseQueries, IAreaQueries areaQueries, IRequestQueries requestQueries, IUserQueries userQueries,IAppUserService appUser)
        {
            _appUser = appUser;
            _franchiseQueries = franchiseQueries;
            _areaQueries = areaQueries;
            _requestQueries = requestQueries;
            _userQueries = userQueries;
        }

        public bool FranchiseMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public RequestFilterSortModel Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        public MultiSelectList Franchises { get; set; }

        public List<RequestStatusModel> Statuses { get; set; }
        public List<SourceAppModel> Sources { get; set; }

        public List<UserModel> SelectedUsers { get; set; }

        public MultiSelectList Pincodes { get; set; }

        public List<LocalityModel> SelectedLocalities { get; set; }

        public List<CityModel> SelectedCities { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            FranchiseMode = _appUser.Current?.FranchiseId != null;
            if (FranchiseMode)
            {
                Pincodes = new MultiSelectList(await _franchiseQueries.AllPincodesOfFranchiseAsync(_appUser.Current.FranchiseId.Value, cancellationToken), Filter?.Pincode);
            }
            else
            {
                if (Filter?.City != null)
                {
                    SelectedCities = await _areaQueries.CitiesByIdsAsync(Filter.City, cancellationToken);
                }
                Franchises = new MultiSelectList(await _franchiseQueries.AllFranchisesAsync(cancellationToken), "Id", "Name", Filter?.Franchise);
            }
            if (Filter?.Locality != null)
                SelectedLocalities = await _areaQueries.LocalitiesByIdsAsync(Filter.Locality, cancellationToken);
            Statuses = await _requestQueries.RequestStatusesAsync(cancellationToken);
            Sources = await _requestQueries.RequestSourceAppAsync(cancellationToken);
            if (Filter?.User != null)
            {
                SelectedUsers = await _userQueries.UsersByIdsAsync(Filter.User, cancellationToken);
            }
        }
        public enum DateType
        {
            [Display(Name = "Scheduled")]
            Visiting,
            [Display(Name = "Submitted")]
            Submitted,
            [Display(Name = "Assigned")]
            Assigned,
            [Display(Name = "Handled")]
            Handled,
            [Display(Name = "Handled updated")]
            HandleUpdated,
            [Display(Name = "Cancelled")]
            Cancelled,
            [Display(Name = "Rescheduled Updated")]
            RescheduledUpdated
        }
        public enum SortBy
        {
            [Display(Name = "Submitted Date")]
            Submitted,
            [Display(Name = "Visiting Date")]
            Visiting,
            [Display(Name = "Pincode-Locality")]
            PincodeLocality,
            [Display(Name = "Pincode-Locality-Visiting Date ")]
            PincodeLocalityVisited,
            [Display(Name = "Pincode-Locality-Submitted Date")]
            PincodeLocalitySubmitted
        }
    }
}