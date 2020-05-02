using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Seedwork
{
    internal class FakeSession
        : ISession
    {
        string _sessionId;

        public FakeSession(string sessionId)
        {
            _sessionId = sessionId;
        }

        public FakeSession()
        {
            _sessionId = Guid.NewGuid().ToString();
        }
        public string Id => _sessionId;

        public bool IsAvailable => true;

        public IEnumerable<string> Keys => Enumerable.Empty<string>();

        public void Clear()
        {
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
        }

        public void Set(string key, byte[] value)
        {
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            value = default;
            return false;
        }
    }
}
