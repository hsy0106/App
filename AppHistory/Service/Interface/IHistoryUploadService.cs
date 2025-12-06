using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppHistory.Service.Interface
{
    public interface IHistoryUploadService
    {
        event Action<string> StatusChanged;
        event Action<UploadResult> UploadCompleted;
        //回调方法
        Func<int,UploadResult> UploadDataCallback { get; set; }


        void Start();
        void Stop();
        void UploadManually();
        bool isRunning { get; }

    }
}
