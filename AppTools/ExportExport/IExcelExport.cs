using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.ExportExport
{
    public interface IExcelExport<T> : IExport<T>
    {
        // Excel特有的方法
        void SetWorksheet(string sheetName);
        void SetHeaderStyle(string styleName);
        void AutoFitColumns();

        void ExportToExcel(T data);
    }
}
