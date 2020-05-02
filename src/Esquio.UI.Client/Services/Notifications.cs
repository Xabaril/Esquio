using Microsoft.JSInterop;
using System;

namespace Esquio.UI.Client.Services
{
    public interface INotifications
    {
        void Info(string title, string message);
        void Warning(string title, string message);
        void Success(string title, string message);
        void Error(string title, string message);
        void Remove();
        void Clear();
    }

    public class Notifications : INotifications
    {
        private readonly IJSInProcessRuntime _jsInProcessRuntime;

        public Notifications(IJSRuntime jsRuntime)
        {
            _jsInProcessRuntime = jsRuntime as IJSInProcessRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public void Info(string title, string message) => _jsInProcessRuntime.InvokeVoid("toastr.info", message, title);
        public void Warning(string title, string message) => _jsInProcessRuntime.InvokeVoid("toastr.warning", message, title);
        public void Success(string title, string message) => _jsInProcessRuntime.InvokeVoid("toastr.success", message, title);
        public void Error(string title, string message) => _jsInProcessRuntime.InvokeVoid("toastr.error", message, title);
        public void Remove() => _jsInProcessRuntime.InvokeVoid("toastr.remove");
        public void Clear() => _jsInProcessRuntime.InvokeVoid("toastr.clear");
    }
}
