using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.AdminPortal.Charts
{
    public class LineChart
    {
        public List<string> labels { get; set; }
        public List<LineDataset> datasets { get; set; }
        public LineChart()
        {
            labels = new List<string>();
            datasets = new List<LineDataset>();
        }
    }
    public class LineDataset
    {
        public string label { get; set; }

        public bool showLine { get; set; }

        public string backgroundColor { get; set; }

        public string borderColor { get; set; }

        public int borderWidth { get; set; }

        public bool fill { get; set; }

        public List<int> data { get; set; }

        public int pointRadius { get; set; }

        public int pointHoverRadius { get; set; }

        public LineDataset()
        {
            showLine = true;
            fill = true;
            data = new List<int>();
            borderWidth = 1;
            pointHoverRadius = 2;
            pointRadius = 0;
        }
    }
}