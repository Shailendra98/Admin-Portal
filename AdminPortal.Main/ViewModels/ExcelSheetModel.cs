using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TKW.AdminPortal.ViewModels
{
    public class ExcelSheetModel<T>
    {
        public string ExcelSheetName { get; set; }

        public DataTable Data { get; set; }

        public ExcelSheetModel(string sheetname, List<T> ModelList)
        {

            ExcelSheetName = sheetname;
            Data = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // Headers
            foreach (PropertyInfo prop in Props)
            {
                /* Adding Headers For Datatables */
                if (Attribute.IsDefined(prop, typeof(DisplayNameAttribute)))
                {
                    DisplayNameAttribute? dna = (DisplayNameAttribute?)Attribute.GetCustomAttribute(prop, typeof(DisplayNameAttribute));
                    if (dna != null)
                        Data.Columns.Add(dna.DisplayName);
                }
                else
                    Data.Columns.Add(prop.Name);
            }
            //Adding  Rows To DataTable
            foreach (var item in ModelList)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null)!;
                }
                Data.Rows.Add(values);
            }
        }
        public ExcelSheetModel(List<T> modellist)
        {
            ExcelSheetName = "Sheet";
            Data = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // Headers
            foreach (PropertyInfo prop in Props)
            {
                /* Adding Headers For Datatables */
                if (Attribute.IsDefined(prop, typeof(DisplayNameAttribute)))
                {
                    DisplayNameAttribute? dna = (DisplayNameAttribute?)Attribute.GetCustomAttribute(prop, typeof(DisplayNameAttribute));
                    if (dna != null)
                        Data.Columns.Add(dna.DisplayName);
                }
                else
                    Data.Columns.Add(prop.Name);
            }
            //Adding  Rows To DataTable
            foreach (var item in modellist)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null)!;
                }
                Data.Rows.Add(values);
            }

        }
    }
}
