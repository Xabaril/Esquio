using Esquio.UI.Api.Infrastructure.Data.Entities;

namespace FunctionalTests.Esquio.UI.Api.Seedwork.Builders
{
    public class TagBuilder
    {
        private string _name = "tag";

        public TagBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TagEntity Build()
        {
            return new TagEntity(_name);
        }
    }
}
