// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace AdminPortal_Component.AdminPortal_Components.AdminPortal_Component.Dashboard_Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using AdminPortal_Component;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using AdminPortal_Component.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common.Axes;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common.Axes.Ticks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common.Handlers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Common.Time;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Util;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\_Imports.razor"
using ChartJs.Blazor.Interop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\Dashboard-Components\AssignedRequestComponent.razor"
using ChartJs.Blazor.LineChart;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\Dashboard-Components\AssignedRequestComponent.razor"
using AdminPortal_Component.SharedComponents;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/Linechart")]
    public partial class AssignedRequestComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\Sameer\source\Repositories\The-Kabadiwala\TKW\src\web\AdminPortal\AdminPortal.Components\AdminPortal-Components\AdminPortal-Component\Dashboard-Components\AssignedRequestComponent.razor"
       
    private const int InitalCount = 6;
    private LineConfig _config;
    private Chart _chart;

    protected override void OnInitialized()
    {
        _config = new LineConfig
        {
            Options = new LineOptions
            {
                Responsive = true,
                Legend = new Legend
                {
                    Display = false,
                    Position = Position.Right
                },
                Title = new OptionsTitle
                {
                    Display = false,
                    Text = "ChartJs.Blazor Line Chart"
                },
                Tooltips = new Tooltips
                {
                    Mode = InteractionMode.Nearest,
                    Intersect = true
                },
                Hover = new Hover
                {
                    Mode = InteractionMode.Nearest,
                    Intersect = true
                },
                

            }
        };

        IDataset<int> dataset1 = new LineDataset<int>(SampleUtils.RandomScalingFactor(InitalCount))
        {
            Label = "Request Count",
            BackgroundColor = ColorUtil.ColorString(92, 198, 65, 0.4),
            BorderColor = ColorUtil.ColorString(92, 198, 65, 1),
            Fill = true,
        };


        _config.Data.Labels.AddRange((SampleUtils.SampleSource.Take(InitalCount)));
        _config.Data.Datasets.Add(dataset1);
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
