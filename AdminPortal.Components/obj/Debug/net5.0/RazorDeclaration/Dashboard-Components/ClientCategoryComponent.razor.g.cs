// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace TKW.AdminPortal.Componentss.Dashboard_Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using TKW.AdminPortal.Component;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using TKW.AdminPortal.Component.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common.Axes;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common.Axes.Ticks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common.Handlers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Common.Time;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Util;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\_Imports.razor"
using ChartJs.Blazor.Interop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\Dashboard-Components\ClientCategoryComponent.razor"
using ChartJs.Blazor.PieChart;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\Dashboard-Components\ClientCategoryComponent.razor"
using System.Drawing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\Dashboard-Components\ClientCategoryComponent.razor"
using TKW.AdminPortal.Components.SharedComponents;

#line default
#line hidden
#nullable disable
    public partial class ClientCategoryComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\Dashboard-Components\ClientCategoryComponent.razor"
       
    private PieConfig _config;

    protected override void OnInitialized()
    {

        _config = new PieConfig()
        {

            Options = new PieOptions
            {

                Responsive = true,
                Legend = new Legend
                {
                    Position = Position.Right
                },
                Title = new ChartJs.Blazor.Common.OptionsTitle
                {
                    Display = false,
                    Text = "ChartJs.Blazor Pie Chart"
                }

            }
        };

        foreach (string color in new[] { "HouseHold", "Office/Shop", "Institute", "Food", "Mall/Outlet", "Other" })
        {
            _config.Data.Labels.Add(color);
        }

        PieDataset<int> dataset = new PieDataset<int>(new[] { 6, 5, 3, 7, 2, 1 })
        {
            BackgroundColor = new[]
            {
            ColorUtil.ColorString(92, 198, 65, 1), // Slice 1 aka "Red"
            ColorUtil.ColorString(92, 198, 65, 0.7), // Slice 2 aka "Yellow"
            ColorUtil.ColorString(92, 198, 65, 0.5), // Slice 3 aka "Green"
            ColorUtil.ColorString(92, 198, 65, 0.3), // Slice 4 aka "Blue"
            ColorUtil.ColorString(92, 198, 65, 0.1),
            ColorUtil.ColorString(92, 198, 65, 0.6),
            ColorUtil.ColorString(92, 198, 65, 0.4),
        }
        };

        _config.Data.Datasets.Add(dataset);
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
