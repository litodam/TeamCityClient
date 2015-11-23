namespace TeamCityClient.Net
{
    using System;
    using System.Net;

    public class PreauthenticatedWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.PreAuthenticate = true;

            return request;
        }
    }
}
