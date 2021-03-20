using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class RequestSourceModel
    {
        public int UserPortal { get; set; }
        public int AdminPortal { get; set; }
        public int PickupBoyApp { get; set; }
        public int UserApp { get; set; }
    }
}
