using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class PurchaseAndSellMaterialList
    {
        public List<PurchaseAndSellMaterialModel> PurchaseList {get; set;}

        public List<PurchaseAndSellMaterialModel> SellList { get; set; }
    }
}
