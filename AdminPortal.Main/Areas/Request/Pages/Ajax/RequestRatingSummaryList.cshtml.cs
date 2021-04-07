using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax
{
    public class RequestRatingSummaryListModel : PageModel
    {
        private readonly IRequestQueries _requestQueries;

        private readonly IAppUserService _appUser;

        public RequestRatingSummaryListModel(IRequestQueries requestQueries, IAppUserService appUser)
        {
            _requestQueries = requestQueries;
            _appUser = appUser;
        }

        public RequestRatingSummaryModel RatingSummary { get; set; }


        public int TotalSellerRatingCount { get; set; }
        public int TotalPickupBoyRatingCount { get; set; }

        public decimal SellerRatingStarAverage { get; set; }
        public decimal PickupBoyRatingStarAverage { get; set; }


        [BindProperty(SupportsGet = true)]
        public RequestRatingFilterSortModel Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? pageSize { get; set; }

        public async Task OnGetAsync()
        {
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseIds = new List<int> { _appUser.Current.FranchiseId.Value };
            RatingSummary = await _requestQueries.RequestRatingSummaryAsync(Filter);
            CalculateData();
        }

        public async Task OnPostAsync()
        {
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseIds = new List<int> { _appUser.Current.FranchiseId.Value };
            RatingSummary = await _requestQueries.RequestRatingSummaryAsync(Filter);
            CalculateData();
        }

        private void CalculateData()
        {
            if (RatingSummary != null)
            {
                TotalSellerRatingCount = RatingSummary.SellerCounts.Sum(m => m.RatingCount);
                TotalPickupBoyRatingCount = RatingSummary.PickupBoyCounts.Sum(m => m.RatingCount);
                PickupBoyRatingStarAverage = TotalPickupBoyRatingCount == 0 ? 0 : RatingSummary.PickupBoyCounts.Sum(m => m.RatingCount * m.Star) / (decimal)TotalPickupBoyRatingCount;
                SellerRatingStarAverage = TotalSellerRatingCount == 0 ? 0 : RatingSummary.SellerCounts.Sum(m => m.RatingCount * m.Star) / (decimal)TotalSellerRatingCount;
            }
        }
    }
}
