using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using TKW.ApplicationCore.Contexts.InventoryContext.DTOs;
using System.ComponentModel.DataAnnotations;

namespace TKW.AdminPortal.Areas.Warehouse.Pages.Ajax
{
    public class PurchaseTabModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateRangeFilterModel Filter { get; set; }
        public void OnGet()
        {

        }
    }
}
