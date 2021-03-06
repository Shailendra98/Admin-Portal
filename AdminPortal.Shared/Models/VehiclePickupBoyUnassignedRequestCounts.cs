using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
  public  class VehiclePickupBoyUnassignedRequestCounts
    {
        public int TotalPickupBoy { get; set; }
        public int FreePickupBoy { get; set; }
        public int TotalVehicle { get; set; }
        public int FreeVehicle { get; set; }
        public int PendingRequest { get; set; }
        public int UnassignedRequest { get; set; }
    }
}
