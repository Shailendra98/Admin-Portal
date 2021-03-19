using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Components.Model
{
    public class AddressModel
    {
        public int FranchiseId { get; set; }
        public int Home { get; set; }
        public int Shop { get; set; }
        public int Office { get; set; }
        public int MallOutlete { get; set; }
        public int Institute { get; set; }
        public int FoodOutlet { get; set; }
        public int Other { get; set; }
    }
}
