using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class AddressTypeModel
    {
        public int Home { get; set; }
        public int Shop { get; set; }
        public int Office { get; set; }
        public int MallOutlete { get; set; }
        public int Institute { get; set; }
        public int FoodOutlet { get; set; }
        public int Other { get; set; }
    }
}
