using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Extensions;
using TKW.AdminPortal.ViewModels;
using TKW.Queries.DTOs.Performance;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.Report.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPerformanceQueries _performanceQueries;
        public IndexModel(IPerformanceQueries performanceQueries)
        {
            _performanceQueries = performanceQueries;
        }

        public string ErrorMessagePickupboy { get; set; }
        public string ErrorMessageRequest { get; set; }

        [BindProperty(SupportsGet = true)]
        public PerformanceFilterSortModel Filter { get; set; }
        public List<PickupboyPerformanceModel> PickupboyPerformance { get; set; }
        public List<CancelledCompletedRequestModel> RequestList { get; set; }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken, string submit)
        {
            switch (submit)
            {
                case "pickupboy":
                    if (Filter?.StartDate != null && Filter?.EndDate != null)
                    {
                        PickupboyPerformance = await _performanceQueries.FiteredAndSortedPickupBoyPerformanceAsync(Filter, cancellationToken);
                        return this.Excel("PurchaseSummaryExcel", new ExcelSheetModel<PickupboyPerformanceModel>(PickupboyPerformance));
                    }
                    else
                    {
                        ErrorMessagePickupboy = "Please select date range.";
                    }
                    break;
                case "request":
                    if (Filter?.StartDate != null && Filter?.EndDate != null)
                    {
                        RequestList = await _performanceQueries.FiteredAndSortedRequsetPerformanceAsync(Filter, cancellationToken);
                        return this.Excel("RequestSummaryExcel", new ExcelSheetModel<CancelledCompletedRequestModel>(RequestList));
                    }
                    else
                    {
                        ErrorMessageRequest = "Please select date range.";
                    }
                    break;
            }
            return null;
        }
    }
}
