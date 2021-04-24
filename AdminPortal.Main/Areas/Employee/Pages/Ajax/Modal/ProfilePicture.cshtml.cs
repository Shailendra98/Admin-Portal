using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.AccountContext.Services;
using TKW.Queries.Interfaces;
using TKW.ApplicationCore.Identity;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal
{
    public class ProfilePictureModel : PageModel
    {
        private readonly IAppUserService _appUser;
        private readonly IEmployeeQueries _employeeQueries;
        private readonly IUserService _userService;
        public ProfilePictureModel(IAppUserService appUser, IEmployeeQueries employeeQueries, IUserService userService)
        {
            _appUser = appUser;
            _employeeQueries = employeeQueries;
            _userService = userService;
        }

        public string ErrorMessage { get; set; }

        public bool IsDone { get; set; }

        [BindProperty(SupportsGet = true)]
        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public byte RoleId { get; set; }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var employee = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
            if (employee == null || employee.FranchiseId != _appUser.Current.FranchiseId)
                return Content(ModalUtils.GenerateContent("Employee Picture", "<div class='alert alert-danger'>Employee does not exist or you don't permission to change the picture of employee.</div>", "").ToString());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
                if (employee == null || employee.FranchiseId != _appUser.Current.FranchiseId)
                    return Content(ModalUtils.GenerateContent("Employee Picture", "<div class='alert alert-danger'>Employee does not exist or you don't permission to change the picture of employee.</div>", "").ToString());
                
                using var ms = new MemoryStream();
                Photo.CopyTo(ms);
                var result = await _userService.UploadProfilePictureAsync(Id, ms.ToArray(), cancellationToken);
                if (result.IsSuccess)
                {
                    RoleId = (byte)employee.Role.Id;
                    IsDone = true;
                }
                else
                    ErrorMessage = result.Error.Message;
            }
            return Page();
        }
    }
}
