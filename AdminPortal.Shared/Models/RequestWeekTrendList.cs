using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class RequestWeekTrendList
    {
        public List<RequestWeekTrendModel> CancelledRequestList { get; set; }
        public List<RequestWeekTrendModel> ScheduledRequestList { get; set; }
        public List<RequestWeekTrendModel> HandledRequestList { get; set; }
    }
}
