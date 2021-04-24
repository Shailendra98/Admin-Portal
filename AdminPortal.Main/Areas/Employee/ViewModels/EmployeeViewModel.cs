using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.Contexts.FranchiseContext.Aggregates.EmployeeAggregate;
using TKW.Queries.DTOs.Franchise;

namespace TKW.AdminPortal.Areas.Employee.ViewModels
{
    /// <summary>
    /// View model for displaying Employee detalis
    /// </summary>
    public class EmployeeViewModel
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee Status
        /// </summary>
        public EmployeeStatus Status { get; set; }

        /// <summary>
        /// Employee's Manager name (if applicable)
        /// </summary>
        public string? ManagerName { get; set; }

        /// <summary>
        /// Employee's picture name
        /// </summary>
        public string? PictureName { get; set; }

        /// <summary>
        /// Mobile number of the employee
        /// </summary>
        public string? MobileNo { get; set; }

        /// <summary>
        /// Employee's Role
        /// </summary>
        public UserRole Role { get; set; }
    }
}
