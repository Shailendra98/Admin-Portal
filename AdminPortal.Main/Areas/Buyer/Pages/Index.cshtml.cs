using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TKW.AdminPortal.Areas.Buyer.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty(SupportsGet =true)]
        public int BuyerId { get; set; }
        public void OnGet()
        {

        }
    }
}
