using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TKW.AdminPortal.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("imginput")]
    public class ImageInput : TagHelper
    {
        public bool Preview { get; set; }

        public int PreviewWidth { get; set; }

        
        public string Name { get; set; }

        public string ButtonClass { get; set; }

        public string Id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var Id = Name.Replace("-", "_").Replace(".", "_");
            if (string.IsNullOrWhiteSpace(Name)) Name = "Picture";
            var id = string.IsNullOrWhiteSpace(Id)?Name.Replace("-", "_").Replace(".", "_") : Id;
            var html = "";
            var img = "";
            var input = "<input type='file' accept='image/*' class='inputfile' data-val='true' data-val-extension='Please select JPG, PNG or BMP file.' data-val-extension-extension='jpg|png|bmp' name='" + Name + "' id='" + id + "'>";
            var label = "<label for='" + id + "' class='" + ButtonClass + "'><span>Choose a file</span></label>";
            //var script = new TagBuilder("script");
            string content = "<script>$(document).ready(function(){"
                            + "$('#" + id + "').on('change',function(e)"
                            + "{"
                            + "  var $input = $(this),"
                            + "  $label = $input.next('label'), "
                            + "  labelVal = $label.html(); "
                            + "         var fileName = '';"
                            + "         if (this.files && this.files.length > 1) "
                            + "               fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length); "
                            + "         else if (e.target.value)"
                            + "           fileName = e.target.value.split('\\\\').pop();"
                            + "     if (fileName)"
                            + "			$label.find('span').html(fileName);"
                            + "	else"
                            + "	$label.html(labelVal);";
            
            if (Preview)
            {
                img = "<div class='imgpreview' style='display:none;margin-bottom:5px;'><img class='img-thumbnail' width='" + PreviewWidth + "' scr='' alt='Image Preview'></div>";
                content += "if (this.files && this.files[0]) {"
                                + "    var reader = new FileReader();"
                                + "  reader.onload = function(e) {"
                                + "   $('#div_" + id + "').find('.imgpreview').slideDown('50',function(){$(this).find('img').attr('src', e.target.result);});"
                                + "    } } "
                                //+ "else{$('#div_" + Id + "').find('.imgpreview').hide(); }"
                                + "  reader.readAsDataURL(this.files[0]);";
            }
            content  += "});"
                            + "	$('#" + id + "')"
                            + ".on('focus', function(){ $(this).addClass('has-focus'); })"
                            + ".on('blur', function(){ $(this).removeClass('has-focus'); });"
                            + " }); "+"</script>";
            html += "<div class='text-center' id='div_" + id + "'>" + img + input + label + "</div>" + content;
            output.Content.SetHtmlContent(html);
            output.TagName = "div";
        }
    }
}
