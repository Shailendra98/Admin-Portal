using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class PaymentMethodModel
    {
        public int UserWallet { get; set; }
        public int Cash { get; set; }
        public int UPI { get; set; }
        public int IMPS { get; set; }
        public int PaytmWallet { get; set; }
    }
}
