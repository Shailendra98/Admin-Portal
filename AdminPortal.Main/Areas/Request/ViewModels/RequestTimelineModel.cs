using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class RequestTimelineModel
    {
        
        public RequestTimelineTypes Type { get; set; }

        public long? Id { get; set; }

        public DateTime? Time { get; set; }

        public string IconClass { get; set; }
        
    }
    public enum RequestTimelineTypes
    {
        Submission,
        SubmissionAndSchedule,
        Schedule,
        Assignment,
        Cancellation,
        Handle,
        Payment,
        Store
    }

}
