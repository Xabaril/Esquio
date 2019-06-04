using Esquio.EntityFrameworkCore.Store.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class ApiKeyBuilder
    {
        private string _name = "api-key-1";
        private string _description = "default description";
        private string _key = "default-key";

        public ApiKeyBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ApiKeyBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ApiKeyBuilder Withkey(string key)
        {
            _key = key;
            return this;
        }

        public ApiKeyEntity Build()
        {
            return new ApiKeyEntity(_name, _description, _key);
        }
    }
}
