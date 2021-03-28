using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class CancelledAndRescheduledRequestModel
    {
        public int RequestId { get; set; }
        public bool IsCancelled { get; set; }
        public int SourceAppId { get; set; }
        public string SourceAppName { get; set; }
        public int ReasonId { get; set; }
        public string ReasonName { get; set; }
    }
}
