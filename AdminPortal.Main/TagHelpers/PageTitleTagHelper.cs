using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.TagHelpers
{
    [HtmlTargetElement("pagetitle")]
    public class PageTitleTagHelper : TagHelper
    {

        public string Title { get; set; }

        public string Description { get; set; }
        

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "normalheader");
            string content = @"<div class=""hpanel animated fadeInDown"">
                                <div class=""panel-body"">
                                     <a class=""small-header-action"" href="""">
                                        <div class=""clip-header"">
                                            <i class=""fa fa-arrow-up""></i>
                                        </div>
                                    </a>
                                    <div class=""pull-right m-t-sm"">" +
                                    (await output.GetChildContentAsync()).GetContent()
                                    + @"</div> 
                                    <h2 class=""font-light m-b-xs"">" + Title + "</h2>";

            if(!string.IsNullOrWhiteSpace(Description)) {
                content += "<small>" + Description + "</small>";
            }
            content+= "</div ></div>";
            output.Content.SetHtmlContent(content);
        }
    }
}
