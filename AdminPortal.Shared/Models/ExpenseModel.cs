using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.Shared.Models
{
    public class ExpenseModel
    {
        public int? ExpenseId { get; set; }
        public string ExpenseName { get; set; }

        public int ExpenseAmount { get; set; }
    }
}
