using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.TagHelpers
{
    [HtmlTargetElement("modal")]
    [RestrictChildren("modal-body","modal-footer","modal-header","form")]
    public class ModalContainerTagHelper : TagHelper
    {
        public string Id { get; set; }

        public string Class { get; set; }

        public string ContentId { get; set; }

        public string DialogClass { get; set; }

        public bool Loader { get; set; }
        
        public int? Tabindex { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("role", "dialog");
            output.Attributes.Add("id", Id);
            output.Attributes.Add("class", "modal fade " + Class);
            output.Attributes.Add("tabindex", Tabindex ?? -1);

            string content = "<div class='modal-dialog " + DialogClass + "'>" +
                "<div class='modal-content' id='" + ContentId + "'>";
            if (Loader)
                content += "<div class=\"modal-body\">" +
                    "<div class=\"text-center\">" +
                    "<img alt=\"Loading..\" src=\"/images/ripple.gif\"/>" +
                    "</div>" +
                    "</div>";
            else
                content += (await output.GetChildContentAsync()).GetContent();
            content += "</div></div>";
            output.Content.SetHtmlContent(content);
        }
    }
}