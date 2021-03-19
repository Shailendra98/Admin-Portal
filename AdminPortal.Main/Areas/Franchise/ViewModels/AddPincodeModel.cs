using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;

namespace TKW.AdminPortal.Areas.Franchise.ViewModels
{
    public class AddPincodeModel 
    {
        public int Pincode { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public bool Sun { get; set; }
    }
}
