namespace Esquio.UI.Api.Features.Tags.List
{
    public class ListTagResponse
    {
        public ListTagResponse(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
