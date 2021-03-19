using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TKW.AdminPortal.Areas.Employee.ViewModels
{
    public class EmployeeInputViewModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string? Name { get; set; }
       
        [BindProperty]
        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"(\d{10})", ErrorMessage = "Mobile number is invalid.")]
        public string? MobileNo { get; set; }
        
        [BindProperty]
        [Required(ErrorMessage = "Manager is required.")]
        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
     
        public SelectList? PickupManagers { get; set; }

        public bool MobileNoDisabled { get; set; }

        public bool NameDisabled { get; set; }
    }
}
