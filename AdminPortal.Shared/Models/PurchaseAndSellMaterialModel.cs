using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class PurchaseAndSellMaterialModel
    {
        public int? MaterialId { get; set; }
        public string MaterialName { get; set; }
        public decimal MaterialQuantity { get; set; }
        public decimal MaterialAmount { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
    }
}
