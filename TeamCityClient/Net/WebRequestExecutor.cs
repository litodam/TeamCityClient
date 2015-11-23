namespace TeamCityClient.Net
{
    using System;
    using System.Net;

    public class WebRequestExecutor : IWebRequestExecutor
    {
        private CredentialCache credentials;

        public WebRequestExecutor()
        {
            this.credentials = new CredentialCache();
        }

        public void SetupBasicAuthCredentials(string url, string username, string password)
        {
            this.credentials.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
        }

        public byte[] UploadData(string url, byte[] data)
        {
            var client = this.CreateWebClient(true);
            return client.UploadData(url, "POST", data);
        }

        public byte[] DownloadData(string url)
        {
            var client = this.CreateWebClient(false);
            return client.DownloadData(url);
        }

        private WebClient CreateWebClient(bool includeUploadHeaders)
        {
            var client = new PreauthenticatedWebClient();
            client.Credentials = this.credentials;

            if (includeUploadHeaders)
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/xml");
            }

            return client;
        }
    }
}