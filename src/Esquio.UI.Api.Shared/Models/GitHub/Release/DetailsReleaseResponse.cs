using System;

namespace Esquio.UI.Api.Shared.Models.GitHub.Release
{
    public class DetailsReleaseResponse
    {
        public string html_url { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public string name { get; set; }
        public bool draft { get; set; }
        public DetailsReleaseResponseAuthor author { get; set; }
        public DateTime published_at { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }
    }

    public class DetailsReleaseResponseAuthor
    {
        public string login { get; set; }
        public string avatar_url { get; set; }
    }
}
