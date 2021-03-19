using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Responses;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates.UserAggregate;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;
using TKW.ApplicationCore.Contexts.FranchiseContext.Queries;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IAppUserService _appUser;

        public EmployeeController(IEmployeeQueries employeeQueries, IAppUserService appUser)
        {
            _employeeQueries = employeeQueries;
            _appUser = appUser;
        }

        [Authorize]
        [Route("~/ajax/search/employee/pickup")]
        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult?> SearchActivePickupEmployees(string q, int? page, CancellationToken cancellationToken)
        {
            var users = await _employeeQueries.SearchActiveEmployeesAsync(q, _appUser.Current.FranchiseId, new List<UserRole> { UserRole.FranchiseAdmin,UserRole.PickupManager,UserRole.PickupBoy }, page ?? 1, 10, cancellationToken);
            if (users == null) return null;
            return Json(new PagedJsonResponse<EmployeeModel>
            {
                Data = users,
                Page = users.PageNumber,
                Size = users.PageSize,
                Total = users.TotalItemCount
            });

        }
    }
}