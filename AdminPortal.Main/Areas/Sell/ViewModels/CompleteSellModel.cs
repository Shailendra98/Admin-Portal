using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;

namespace TKW.AdminPortal.Areas.Sell.ViewModels
{
    public class CompleteSellModel
    {
        
        /// <summary>
        /// List of material with quantity and rate
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage = "Materials are required.")]
        [Display(Name = "Materials")]
        [MinLength(1, ErrorMessage = "Atleast one material is required.")]
        public List<SellMaterialRateQuantityInputModel> Materials { get; set; }

       
        /// <summary>
        /// Whether it is GST Bill
        /// </summary>
        [BindProperty]
        public bool IsGST { get; set; }

        /// <summary>
        /// Whether type of GST is IGST
        /// </summary>
        [BindProperty]
        public bool IsIGST { get; set; }

        /// <summary>
        /// Sell Comment
        /// </summary>
        [BindProperty]
        public string? Comment { get; set; }

    }
}
