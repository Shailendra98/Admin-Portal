﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
  public  class IdleVehiclePickupBoyModel
    {
        public int TotalPickupBoy { get; set; }
        public int FreePickupBoy { get; set; }
        public int TotalVehicle { get; set; }
        public int FreeVehicle { get; set; }
        
        public int TodaysRequest { get; set; }
        public int Unassigned { get; set; }

    }
}
