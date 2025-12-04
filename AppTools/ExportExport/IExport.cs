using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTools.ExportExport
{
    public interface IExport<T>
    {
        event Action<T> ExportCompleted;
        void ExportData(T data);
    }
}
