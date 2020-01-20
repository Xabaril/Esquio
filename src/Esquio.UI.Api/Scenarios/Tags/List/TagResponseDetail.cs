namespace Esquio.UI.Api.Scenarios.Tags.List
{
    public class TagResponseDetail
    {
        protected TagResponseDetail() { }

        public TagResponseDetail(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
