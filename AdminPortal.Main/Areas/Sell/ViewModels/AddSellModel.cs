using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TKW.AdminPortal.ViewModels;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;
using TKW.ApplicationCore.Contexts.SellContext.DTOs;

namespace TKW.AdminPortal.Areas.Sell.ViewModels
{
    /// <summary>
    /// Data model for adding sell
    /// </summary>
    public class AddSellModel
    {
        /// <summary>
        /// Buyer Id
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage="Buyer is required.")]
        [Display(Name ="Buyer")]
        public int? BuyerId { get; set; }

        /// <summary>
        /// Warehouse Id
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage ="Warehouse is required.")]
        [Display(Name = "Warehouse")]
        public int? WarehouseId { get; set; }

        /// <summary>
        /// Handlers' Ids
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage ="Handlers are required.")]
        [Display(Name = "Handlers")]
        [MinLength(1,ErrorMessage ="Atleast one pickup boy is required.")]
        public List<int> HandlerIds { get; set; }

        /// <summary>
        /// List of material with quantity and rate
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage ="Materials are required.")]
        [Display(Name = "Materials")]
        [MinLength(1,ErrorMessage ="Atleast one material is required.")]
        public List<SellMaterialRateQuantityInputModel> Materials { get; set; }

        /// <summary>
        /// Time when sell is handled
        /// </summary>
        [BindProperty]
        [Required(ErrorMessage ="Handled time is required.")]
        [Display(Name = "Handled time")]
        public DateTime? HandledTime { get; set; }

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

        public SelectList? Buyers { get; set; }

        public List<UserModel>? Handlers { get; set; }

        public SelectList? Warehouses { get; set; }

    }
}
