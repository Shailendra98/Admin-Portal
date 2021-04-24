using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TKW.AdminPortal.ViewModels;
using TKW.Queries.DTOs.Account;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class HandleModel
    {
        [Display(Name = "Handled Time")]
        [Required(ErrorMessage = "Handled time is required.")]
        public DateTime HandleEndTime { get; set; }

        [Display(Name = "Handlers")]
        [Required(ErrorMessage = "At least one handler is required.")]
        public List<int> HandlerIds { get; set; }

        [Required(ErrorMessage = "Materials are required.")]
        public List<MaterialRateQuantityInputModel> Materials { get; set; }

        public List<UserModel>? Handlers { get; set; }
    }
}
