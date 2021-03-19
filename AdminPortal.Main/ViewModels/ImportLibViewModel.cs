using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.Enums;

namespace TKW.AdminPortal.ViewModels
{
    public class ImportLibViewModel
    {
        public string Js { get; set; }

        public string Css { get; set; }

        public ClientSideLib Lib { get; set; }
        
        public ImportLibViewModel(ClientSideLib lib)
        {
            Lib = lib;
            switch (lib)
            {
                case ClientSideLib.DatetimePicker:
                    Js = "/js/datetimepicker.min.js";
                    Css = "/css/datetimepicker.min.css";
                    break;
                case ClientSideLib.BlueImpGallery:
                    Js = "/js/blueimpgallery.min.js";
                    Css = "/css/blueimpgallery.min.css";
                    break;
                case ClientSideLib.SearchElem:
                    Js = "/js/searchelem.min.js";
                    Css = "/css/native.min.css";
                    break;
                case ClientSideLib.ChartJs:
                    Js = "/js/chart.min.js";
                    break;
            }
        }
    }
}
