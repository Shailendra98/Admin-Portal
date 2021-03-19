using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.Extensions;
using TKW.AdminPortal.ViewModels;

namespace TKW.AdminPortal.TagHelpers
{
    [HtmlTargetElement("lib",TagStructure =TagStructure.WithoutEndTag)]
    public class LibTagHelper : TagHelper
    {
        public AdminPortal.Enums.ClientSideLib Name { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public async override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = new ImportLibViewModel(Name);
            var sw = new StringWriter();
            
            ViewDataDictionary viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model=model };

            var viewContext = new ViewContext(ViewContext, ViewContext.View, viewData, ViewContext.TempData, sw, new HtmlHelperOptions());
            var html = new HtmlString(await viewContext.RenderPartialViewAsync("_Lib"));
            output.PreElement.SetHtmlContent(html);
        }
    }
}
