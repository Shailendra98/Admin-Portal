using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Areas.User.Pages.Ajax;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;
using TKW.Queries.DTOs.Account;
using TKW.Queries.Interfaces;


namespace TKW.AdminPortal.Areas.User.Pages
{
    public class IndexModel : PageModel
    {
        private IUserQueries _userQueries;
        public IndexModel(IUserQueries userService)
        {
            _userQueries = userService;
        }

        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserFilterSortModel Filter { get; set; }
        public List<AddressType> AddressTypes { get; set; }


        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            AddressTypes = Enumeration.GetAll<AddressType>().ToList();
        }


        public enum DateTypeOption
        {
            [Display(Name = "Joining Date")]
            JoiningDate,
            [Display(Name = "Last Request Handle Date")]
            LastRequestHandledDate,
        }

        public enum SortByOption
        {
            [Display(Name = "Joining Date")]
            JoiningDate,
            [Display(Name = "Name")]
            Name,
            [Display(Name = "Request Count")]
            RequestCount,
            [Display(Name = "LastRequestHandleDate")]
            LastRequestHandleDate,
        }
    }
}
