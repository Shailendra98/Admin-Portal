using System;
using System.Collections.Generic;
using System.Text;

namespace TKW.AdminPortal.ViewModels
{
    /// <summary>
    /// Detials of available date and slots
    /// </summary>
    public class AvailableDateAndSlotModel
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// List of Available slots
        /// </summary>
        public IEnumerable<int> SlotIds { get; set; }
    }
}