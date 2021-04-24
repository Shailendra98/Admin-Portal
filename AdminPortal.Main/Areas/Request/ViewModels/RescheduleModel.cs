using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TKW.Queries.DTOs.Purchase;

namespace TKW.AdminPortal.Areas.Request.ViewModels
{
    public class RescheduleModel
    {
        [Display(Name = "Reason")]
        [Required(ErrorMessage = "Reason is required.")]
        public int? ReasonId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime? Date { get; set; }

        [Display(Name = "Slot")]
        [Required(ErrorMessage = "Time Slot is required.")]
        public byte TimeSlotId { get; set; }

        public List<TimeSlotModel>? TimeSlots { get; set; }

        public List<RequestScheduleReasonModel>? Reasons { get; set; }
    }
}
