using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.ViewModels
{
    public class AvailableScheduleModel
    {
        /// <summary>
        /// List of all slots
        /// </summary>
        public Dictionary<int,string> Slots { get; set; }

        /// <summary>
        /// List of all available dates with list of all availble slot ids.
        /// </summary>
        public List<AvailableDateAndSlotModel> Dates { get; set; }

        public AvailableScheduleModel()
        {
            Slots = new Dictionary<int, string>();
            Dates = new List<AvailableDateAndSlotModel>();
        }
    }
}
