using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TKW.AdminPortal.TagHelpers
{
    public class ModalHeaderTagHelper : TagHelper
    {
        public bool CloseButton { get; set; }

        public string Title { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "modal-header");
            string Content = "";
            Content += "<h4 class='modal-title'>" + Title + "</h4>";
            if (CloseButton)
                Content += "<button type='button' class='btn-close float-end' data-bs-dismiss='modal' aria-label='Close'></button>";
          
            output.Content.SetHtmlContent(Content);

        }
    }
}
