using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;

namespace Esquio.UI.Client.Services
{
    public interface ILocalStorage
    {
        void SetItem(string key, object data);
        void RemoveItem(string key);
        T GetItem<T>(string key);
    }

    public class LocalStorage : ILocalStorage
    {
        private readonly IJSInProcessRuntime _jsInProcessRuntime;

        public LocalStorage(IJSRuntime jsRuntime)
        {
            _jsInProcessRuntime = jsRuntime as IJSInProcessRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public void SetItem(string key, object data)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));
            _ = data ?? throw new ArgumentNullException(nameof(data));

            _jsInProcessRuntime.Invoke<object>("localStorage.setItem", key, JsonConvert.SerializeObject(data));
        }

        public void RemoveItem(string key)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            _jsInProcessRuntime.Invoke<object>("localStorage.removeItem", key);
        }

        public T GetItem<T>(string key)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));

            var data = _jsInProcessRuntime.Invoke<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(data))
            {
                return default(T);
            }
                
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
