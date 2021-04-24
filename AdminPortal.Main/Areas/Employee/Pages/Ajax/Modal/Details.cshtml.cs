using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TKW.AdminPortal.Areas.Employee.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.Queries.Interfaces;

namespace TKW.AdminPortal.Areas.Employee.Pages.Ajax.Modal
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeQueries _employeeQueries;

        public DetailsModel(IEmployeeQueries employeeQueries)
        {
            _employeeQueries = employeeQueries;
        }

        public EmployeeViewModel Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int Id, CancellationToken cancellationToken)
        {
            var e = await _employeeQueries.EmployeeAsync(Id, cancellationToken);
            if (e == null) return Content(Utils.ModalUtils.GenerateContent("Employee Details", "<div class='alert alert-danger'>Employee does not exist any longer.</div>", "").ToString());
            
            Employee = new EmployeeViewModel
            {
                Id = e.Id,
                MobileNo = e.MobileNo,
                Name = e.Name,
                PictureName = e.PictureName,
                Role = e.Role,
                Status = e.Status
            };
            if (e.Role == UserRole.PickupBoy)
                Employee.ManagerName = (await _employeeQueries.ManagerOfPickupBoyAsync(e.Id, cancellationToken))?.Name;
            return Page();
        }
    }
}
