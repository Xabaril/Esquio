using System;

namespace Esquio.Distributed.Store.DependencyInjection
{
    public sealed class DistributedStoreOptions
    {
        internal bool CacheEnabled = false;

        /// <summary>
        /// Configure if cache is enabled on distributed store.
        /// </summary>
        /// <param name="enabled">If True distributed store use default IDistributedStore configured on container. Else, cache is not enabled.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseCache(bool enabled = false)
        {
            CacheEnabled = enabled;
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
        /// <param name="uri">The Esquio store api key to be used.</param>
        /// <returns>The same configuration to be chained.</returns>
        public DistributedStoreOptions UseApiKey(string apiKey)
        {
            ApiKey = apiKey;
            return this;
        }
    }
}
