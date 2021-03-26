using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class RequestCountsModel
    {
        public int TodaysRequests { get; set; }
        public int Pending { get; set; }
        public int Cancelled { get; set; }
        public int Handled { get; set; }
        public int Onspot { get; set; }
        public int Rescheduled { get; set; }
    }
}
