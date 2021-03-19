using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKW.ApplicationCore.Types;

namespace TKW.AdminPortal.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        public IPagedList List { get; set; }

        public Func<int,string> Url { get; set; }

        public bool Ajax { get; set; } = true;

        public int pageShow { get; set; } = 5;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (List.PageCount == 1) return;
            int diff = pageShow / 2;
            int start = Math.Max(1, List.PageNumber - diff);
            int end = Math.Min(List.PageCount, List.PageNumber + pageShow - diff);
            int pageCount = List.PageCount;
            var content = "<ul class='pagination'>";
            var attr = context.AllAttributes.Where(m => m.Name.StartsWith("data-")).ToList();
            attr.RemoveAll(m => m.Name == "data-ajax-url");
            var attrStr = string.Join(" ", attr.Select(m =>
                 m.Name + "=" + Quote(m.ValueStyle) + m.Value.ToString() + Quote(m.ValueStyle)
             ));
            if (start != 1)
                content += GeneratePageButton("&lt;&lt;", Url(1), attrStr)
                    + GeneratePageButton("&lt;", Url(List.PageNumber - 1), attrStr)
                    + "<li class='page-item disabled'><a class='page-link'>...</a></li>";
            else if (List.HasPreviousPage)
                content += GeneratePageButton("&lt;", Url(List.PageNumber - 1), attrStr);
            for (int i = start; i <= end; i++)
            {
                content += GeneratePageButton(i.ToString(), Url(i), attrStr, i==List.PageNumber);
            }
            if (end != pageCount)
                content += "<li class='page-item disabled'><a class='page-link'>...</a></li>"
                    + GeneratePageButton("&gt;", Url(List.PageNumber + 1), attrStr)
                    + GeneratePageButton("&gt;&gt;", Url(pageCount), attrStr);
            else if (List.HasNextPage)
                content += GeneratePageButton("&gt;", Url(List.PageNumber + 1), attrStr);
            content += "</ul>";
            output.TagName="nav";
            output.Content.SetHtmlContent(new HtmlString(content));
        }

        private string GeneratePageButton(string content,string url, string attributeString,bool isActive=false)
        {
            if (Ajax)
                attributeString += " data-ajax-url='" + url + "'";
            return $"<li {(isActive ? "class='page-item active'" : "class='page-item'")}><a class='page-link' {attributeString} href=\"{(Ajax?"#":url)}\">{content}</a></li>";
        }
        private static string Quote(HtmlAttributeValueStyle style)
        {
            switch (style)
            {
                case HtmlAttributeValueStyle.DoubleQuotes : return "\"";
                default:return "\'";
            }
        }
    }
}
