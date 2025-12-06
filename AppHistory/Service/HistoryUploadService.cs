using AppHistory.Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppHistory.Service
{
    public class HistoryUploadService : IHistoryUploadService, IDisposable
    {
        private BackgroundWorker _uploadWorker;
        private System.Threading.Timer _uploadTimer;

        private bool _isRunning = false;
        public bool IsRunning => _isRunning;

        public event Action<string> StatusChanged;
        public event Action<UploadResult> UploadCompleted;

        public Func<int, UploadResult> UploadDataCallback { get; set; }

        public bool isRunning => _isRunning;

        public void Dispose()
        {
            Stop();
            _uploadTimer.Dispose();
            _uploadWorker.Dispose();

        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _uploadTimer.Change(0, 5000);
            OnStatusChanged("上传服务已启动");
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _uploadTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _uploadWorker.CancelAsync();
            OnStatusChanged("上传服务已停止");
        }

        public void UploadManually()
        {
            if (!_uploadWorker.IsBusy)
            {
                _uploadWorker.RunWorkerAsync();
                OnStatusChanged("手动上传开始...");
            }
        }
        private void UploadTimerCallback(object state)
        {
            if (_isRunning && !_uploadWorker.IsBusy)
            {
                _uploadWorker.RunWorkerAsync();
            }
        }
        private void UploadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (UploadDataCallback == null)
                {
                    e.Result = new UploadResult();
                    return;
                }

                int maxCount = 100;
                e.Result = UploadDataCallback.Invoke(maxCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "上传执行异常");
                e.Result = new UploadResult();
            }
        }

        private void UploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is UploadResult result)
            {
                UploadCompleted?.Invoke(result);
                OnStatusChanged($"上传完成: 成功 {result.SuccessCount}/总数 {result.TotalCount}");
            }
        }

        protected void OnStatusChanged(string msg) =>
            StatusChanged?.Invoke(msg);
    }
    public class UploadResult
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount => TotalCount - SuccessCount;
    }

}
