using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.Controllers;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;
using TKW.ApplicationCore.SeedWorks;

namespace TKW.AdminPortal.ViewModels
{
    public class AddressModel
    {
        [Display(Name = "Address line")]
        [Required(ErrorMessage = "Address Line is required")]
        public string? AddressLine { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [Display(Name = "Locality")]
        [Required(ErrorMessage = "Locality is required.")]
        public int? LocalityId { get; set; }

        [Display(Name ="City")]
        public int? CityId { get; set; }

        public string? LocalityName { get; set;}
        public decimal? LocalityLatitude { get; set; }
        public decimal? LocalityLongitude { get; set; }
        public string? CityName { get; set; }

        public string? Name { get; set; }

        [RegularExpression(@"(\d{10})")]
        public string? MobileNo { get; set; }

        [Required]
        public int? AddressTypeId { get; set; }

        /// <summary>
        /// Additional details (Name and Mobile number of the user)
        /// </summary>
        public bool IncludeNameMobileNo { get; set; }

        public List<AddressType> AddressTypes { get; set; } 

        public bool OnlyLocalities { get; set; }

        
        public AddressModel()
        {

           
            IncludeNameMobileNo = !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(MobileNo);
            AddressTypes = Enumeration.GetAll<AddressType>().ToList();
          
        }
    }
}
