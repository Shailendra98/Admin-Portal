using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Sell.ViewModels
{
    public class SellTimelineModel
    {
        public SellTimelineTypes Type { get; set; }

        public long? Id { get; set; }

        public DateTime? Time { get; set; }

        public string IconClass { get; set; }

        public enum SellTimelineTypes
        {
            Pending,
            Handled,
            Completed,
            Cancelled
        }
    }
}
