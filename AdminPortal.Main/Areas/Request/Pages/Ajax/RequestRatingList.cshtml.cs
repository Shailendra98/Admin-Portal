using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Request.ViewModels;
using TKW.ApplicationCore.Contexts.PurchaseContext.DTOs;
using TKW.ApplicationCore.Contexts.PurchaseContext.Queries;
using TKW.ApplicationCore.Identity;
using TKW.ApplicationCore.Types;

namespace TKW.AdminPortal.Areas.Request.Pages.Ajax
{
    public class RequestRatingListModel : PageModel
    {

        private readonly IRequestQueries _requestQueries;

        private readonly IAppUserService _appUser;

        public RequestRatingListModel(IRequestQueries requestQueries, IAppUserService appUser)
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

        public PagedList<RequestRatingListItemModel> Ratings { get; set; }


        public async Task OnGetAsync(int? pageNo, int? pageSize)
        {
            int size = pageSize == null ? 8 : (pageSize < 5) ? 5 : (pageSize > 100) ? 100 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseIds = new List<int> { _appUser.Current.FranchiseId.Value };
             Ratings = await _requestQueries.FilteredAndSortedRequestRatingsAsync(Filter, pageNo ?? 1, size);
            RatingSummary = await _requestQueries.RequestRatingSummaryAsync(Filter);
            CalculateData();
        }

        public async Task OnPostAsync(int? pageNo, int? pageSize)
        {
            int size = pageSize == null ? 8 : (pageSize < 5) ? 5 : (pageSize > 100) ? 100 : pageSize.Value;
            if (_appUser.Current.FranchiseId.HasValue)
                Filter.FranchiseIds = new List<int> { _appUser.Current.FranchiseId.Value };
            Ratings = await _requestQueries.FilteredAndSortedRequestRatingsAsync(Filter, pageNo ?? 1, size);
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
