using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Areas.Incentive.ViewModels
{
    public class RewardInputModel
    {
        [BindProperty]
        [Display(Name = "Reward")]
        [Required(ErrorMessage ="Reward is Required")]
        public int Reward { get; set; }

        [BindProperty]
        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date is Required")]
        public DateTime StartDateTime { get; set; }

        [BindProperty]
        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End Date is Required")]
        public DateTime EndDateTime { get; set; }

        [BindProperty]
        public bool IsGlobal { get; set; }

        [BindProperty]
        public List<int>? FranchiseIds { get; set; }

    }
}
