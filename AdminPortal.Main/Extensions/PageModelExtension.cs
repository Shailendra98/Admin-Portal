using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKW.AdminPortal.ViewModels;

namespace TKW.AdminPortal.Extensions
{
    public static class PageModelExtension
    {
        public static FileContentResult Excel<T>(this PageModel pageModel, string FileName, ExcelSheetModel<T> sheet)
        {
            DataTable ExcelDataTable = sheet.Data;
            var wb = new XLWorkbook();
            var data = sheet.ExcelSheetName == null ? wb.AddWorksheet(ExcelDataTable) : wb.AddWorksheet(ExcelDataTable, sheet.ExcelSheetName);


            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = FileName + ".Xslx",
            };
        }
    }
}
