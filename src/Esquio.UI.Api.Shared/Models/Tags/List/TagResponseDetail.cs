namespace Esquio.UI.Api.Shared.Models.Tags.List
{
    public class TagResponseDetail
    {
        protected TagResponseDetail() { }

        public TagResponseDetail(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
