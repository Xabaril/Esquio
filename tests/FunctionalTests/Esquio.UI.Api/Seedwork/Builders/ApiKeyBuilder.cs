using Esquio.EntityFrameworkCore.Store.Entities;
using System;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ApiKeyBuilder
    {
        private string _name = "api-key-1";
        private DateTime _validTo = default;
        private string _key = "default-key";

        public ApiKeyBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ApiKeyBuilder WithValidTo(DateTime validTo)
        {
            _validTo = validTo;
            return this;
        }

        public ApiKeyBuilder Withkey(string key)
        {
            _key = key;
            return this;
        }

        public ApiKeyEntity Build()
        {
            return new ApiKeyEntity(_name, _key, _validTo);
        }
    }
}
