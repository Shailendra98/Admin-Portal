using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Extensions;
using TKW.AdminPortal.ViewModels;
using TKW.Queries.DTOs.Report;
using TKW.Queries.Interfaces;
using TKW.SharedKernel.Types;

namespace TKW.AdminPortal.Areas.Report.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IReportQueries _reportQueries;
        public IndexModel(IReportQueries reportQueries)
        {
            _reportQueries = reportQueries;
        }

        public string ErrorMessagePickupboy { get; set; }
        public string ErrorMessageRequest { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDatePickupBoy { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDatePickupBoy { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDateRequest { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDateRequest { get; set; }

        public List<PickupboyPerformanceModel> PickupboyPerformance { get; set; }
        public List<CancelledCompletedRequestModel> RequestList { get; set; }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken, string submit)
        {
            switch (submit)
            {
                case "pickupboy":
                    if (StartDatePickupBoy != null && EndDatePickupBoy != null)
                    {
                        PickupboyPerformance = await _reportQueries.PickupBoyPerformanceListAsync(StartDatePickupBoy.Value, EndDatePickupBoy.Value, cancellationToken);
                        return this.Excel("PurchaseSummaryExcel", new ExcelSheetModel<PickupboyPerformanceModel>(PickupboyPerformance));
                    }
                    else
                    {
                        ErrorMessagePickupboy = "Please select date range.";
                    }
                    break;
                case "request":
                    if (StartDateRequest != null && EndDateRequest != null)
                    {
                        RequestList = await _reportQueries.CancelledCompletesRequestListAsync(StartDateRequest.Value, EndDateRequest.Value, cancellationToken);
                        return this.Excel("RequestSummaryExcel", new ExcelSheetModel<CancelledCompletedRequestModel>(RequestList));
                    }
                    else
                    {
                        ErrorMessageRequest = "Please select date range.";
                    }
                    break;
            }
            return Page();
        }
    }
}
