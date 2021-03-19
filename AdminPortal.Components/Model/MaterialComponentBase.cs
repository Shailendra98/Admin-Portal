using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Components.Model
{
    public class MaterialComponentBase : ComponentBase
{
        public string MaterialName { get; set; }

        public int MaterialCollected { get; set; }

        public int MaterialSold { get; set; }

        public int NetGain { get; set; }
    }
}
