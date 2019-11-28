namespace Esquio.CliTool.Internal
{
    public static class FileResponseExtensions
    {
        public static bool IsSuccessStatusCode(this FileResponse fileResponse)
        {
            return fileResponse.StatusCode >= 200 && fileResponse.StatusCode <= 299;
        }
    }
}
