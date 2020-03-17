using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;

namespace Esquio.UI.Client.Services
{
    public interface ILocalStorage
    {
        void SetItem(string key, object data);
        T GetItem<T>(string key);
    }

    public class LocalStorage : ILocalStorage
    {
        private readonly IJSInProcessRuntime jSInProcessRuntime;

        public LocalStorage(IJSRuntime jSRuntime)
        {
            jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
        }

        public void SetItem(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            jSInProcessRuntime.Invoke<object>("localStorage.setItem", key, JsonConvert.SerializeObject(data));

        }

        public T GetItem<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (jSInProcessRuntime == null)
                throw new InvalidOperationException("IJSInProcessRuntime not available");

            var serialisedData = jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            return JsonConvert.DeserializeObject<T>(serialisedData);
        }
    }
}
