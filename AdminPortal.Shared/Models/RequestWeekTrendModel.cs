using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class RequestWeekTrendModel
    {
        
        public DateTime Date { get; set; }
        public int ScheduledRequestCount { get; set; }
        public int HandledRequestCount { get; set; }
        public int CancelledRequestCount { get; set; }
    }
}

