using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TKW.AdminPortal.Utils;
using TKW.ApplicationCore.Contexts.AccountContext.Aggregates;

namespace TKW.AdminPortal.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("address")]
    public class AddressTagHelper : TagHelper
    {
        public string Name { get; set; }

        public string MobileNo { get; set; }

        public string Addressline { get; set; }

        public string Type { get; set; }
    
        public string Locality { get; set; }

        public string Pincode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "address";
            output.Content.SetHtmlContent(new HtmlString(AddressUtils.GenerateHTML(Addressline, Type , Locality, City, State, Pincode, Name, MobileNo)));
        }
    }
}
