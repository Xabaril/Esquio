using System;

namespace Esquio.Distributed.Store.DependencyInjection
{
    public sealed class DistributedStoreOptions
    {
        internal bool CacheEnabled = false;
        internal TimeSpan? AbsoluteExpirationRelativeToNow = null;
        internal TimeSpan? SlidingExpiration = null;

        /// <summary>
        /// Configure if cache is enabled on distributed store.
        /// </summary>
        /// <param name="enabled">If True distributed store use default IDistributedStore configured on container. Else, cache is not enabled.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseCache(bool enabled = false, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
        {
            CacheEnabled = enabled;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            SlidingExpiration = slidingExpiration;

            return this;
        }

        internal Uri BaseAddress = null;

        /// <summary>
        /// Configure distributed store base address.
        /// </summary>
        /// <param name="uri">The distributed store base address to be used.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseBaseAddress(Uri uri)
        {
            BaseAddress = uri;
            return this;
        }

        /// <summary>
        /// Configure Esquio base address.
        /// </summary>
        /// <param name="uri">The Esquio base address to be used.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseBaseAddress(string uri)
        {
            BaseAddress = new Uri(uri);
            return this;
        }


        internal string ApiKey = null;
        /// <summary>
        /// Configure Esquio api key.
        /// </summary>
        /// <param name="apiKey">The Esquio store api key to be used.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseApiKey(string apiKey)
        {
            ApiKey = apiKey;
            return this;
        }


        internal TimeSpan Timeout = TimeSpan.FromSeconds(100); 
        /// <summary>
        /// Configure Esquio api key.
        /// </summary>
        /// <param name="timeout">The maximiun time than distributed store wait for server response.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions SetTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }
    }
}
