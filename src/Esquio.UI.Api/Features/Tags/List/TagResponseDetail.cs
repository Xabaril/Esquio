namespace Esquio.UI.Api.Features.Tags.List
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
