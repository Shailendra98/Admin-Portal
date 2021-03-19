using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Utils
{
    /// <summary>
    /// Utilities for modal
    /// </summary>
    public static class ModalUtils
    {
        /// <summary>
        /// Generates html string for modal content
        /// </summary>
        /// <param name="title">Title of the modal</param>
        /// <param name="body">Body of the modal</param>
        /// <param name="footer">Footer of the modal</param>
        /// <param name="closeButtonInHeader">Whether to put close button in Header</param>
        /// <param name="closeButtonInFooter">Whether to put close button in Footer</param>
        public static HtmlString GenerateContent(string title, string body, string footer, bool closeButtonInHeader = true,bool closeButtonInFooter=false)
        {
            var h = "";
            if (!string.IsNullOrWhiteSpace(title))
                h = "<div class='modal-header'>" + (closeButtonInHeader ? "<button type='button' class='close' data-dismiss='modal'>&times;</button>" : "") + "<h4 class='modal-title'>" + title + "</h4></div>";
            var b = "<div class='modal-body'>" + body + "</div>";
            var f = "";
            if (!string.IsNullOrWhiteSpace(footer))
                f = "<div class='modal-footer'>" + footer + (closeButtonInFooter ? "<button type='button' class='btn btn-default' data-dismiss='modal'>Close</button>" : "") + "</div>";
            return new HtmlString(h+b+f);
        } 
    }
}
