using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class PickupSessionModel
    {
        public int Id { get; set; }
        public decimal Cash { get; set; }
        public List<PickupBoyModel> PickupBoys { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public int PendingRequests { get; set; }
        public int RescheduledRequests { get; set; }
        public int CancelledRequests { get; set; }
        public int HandledRequests { get; set; }
    }
}
