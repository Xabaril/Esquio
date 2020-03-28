namespace Esquio.UI.Api.Shared.Models.Tags.List
{
    public class TagResponseDetail
    {
        protected TagResponseDetail() { }

        public TagResponseDetail(string name, string color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
