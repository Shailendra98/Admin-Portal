using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.Queries.Interfaces;
using TKW.Queries.DTOs.Franchise;
using Microsoft.AspNetCore.Mvc.Rendering;
using TKW.ApplicationCore.SeedWorks;
using TKW.ApplicationCore.Contexts.AreaContext.Services;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.AdminPortal.Areas.Franchise.ViewModels;
using TKW.ApplicationCore.Contexts.AreaContext.Aggregates.Pincode;
using System.ComponentModel.DataAnnotations;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Franchise.Pages
{
    public class PincodesModel : PageModel
    {

        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }

        
        public void OnGet()
        {
          
        }
    } 
}
