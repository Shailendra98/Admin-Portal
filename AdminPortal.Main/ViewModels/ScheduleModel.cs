using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.ViewModels
{
    public class ScheduleModel
    {
        [Required]
        [Display(Name ="Pickup Date")]
        public DateTime? Date { get; set; }

        [Required]
        [Display(Name ="Time slot")]
        public byte? TimeSlotId { get; set; }

        public AvailableScheduleModel? AvailableSchedule { get; set; }
    }
}
