using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;

namespace TKW.AdminPortal.Areas.Employee.ViewModels
{
    public class EmployeeProfileModel
    {
        [BindProperty]
        public int? Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Photo is required.")]
        [Display(Name = "Photo")]
        public IFormFile Photo { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "MobileNo. is required.")]
        [Display(Name = "MobileNo.")]
        public string MobileNo { get; set; }

        public EmployeeModel EmployeeProfile { get; set; }

      
    }
}
