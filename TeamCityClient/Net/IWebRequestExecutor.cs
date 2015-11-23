namespace TeamCityClient.Net
{
    public interface IWebRequestExecutor
    {
        byte[] DownloadData(string url);

        void SetupBasicAuthCredentials(string url, string username, string password);

        byte[] UploadData(string url, byte[] data);
    }
}
