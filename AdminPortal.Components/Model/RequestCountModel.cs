using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Components.Model
{
    public class RequestCountModel
    {

        public int FranchiseId { get; set; }
        public int Unassigned { get; set; }
        public int Assigned { get; set; }
        public int Cancelled { get; set; }
        public int Handled { get; set; }
        public int Onspot { get; set; }
        public int Rescheduled { get; set; }
    }
}
