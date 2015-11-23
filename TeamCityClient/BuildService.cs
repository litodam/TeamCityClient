namespace TeamCityClient
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using DataContracts;
    using Net;

    public class BuildService : IBuildService
    {
        public const string TestRunnerBuildType = "WindowsAzureStore_WindowsStoreServicesValidation_TestRunner";

        private const string BaseRestUrlTemplate = "{0}/httpAuth/app/rest/{1}";
        private const string BuildQueueEndpoint = "buildQueue";
        private const string BuildQueueDetailsEndpoint = "buildQueue/taskId:{0}";
        private const string BuildDetailsEndpoint = "builds/id:{0}";
        private const string BuildTestOcurrencesListEndpoint = "testOccurrences?locator=build:(id:{0})";
        private const string BuildTestOcurrenceDetailsEndpoint = "testOccurrences/id:{0},build:(id:{1})";
        private const string BuildReferencesForTypeSinceLastBuildEndpoint = "builds?locator=buildType:{0},sinceBuild:{1},running:false,canceled:false,count:9000";

        private readonly string serverUrl;
        private IWebRequestExecutor requestExecutor = null;

        public BuildService()
            : this(new WebRequestExecutor(), ConfigurationManager.AppSettings["TeamCityServerUrl"], ConfigurationManager.AppSettings["TeamCityUserName"], ConfigurationManager.AppSettings["TeamCityPassword"])
        {
        }

        public BuildService(IWebRequestExecutor requestExecutor, string teamCityServerUrl, string teamCityUsername, string teamCityPassword)
        {
            this.serverUrl = teamCityServerUrl;
            this.requestExecutor = requestExecutor;
            this.requestExecutor.SetupBasicAuthCredentials(this.serverUrl, teamCityUsername, teamCityPassword);
        }

        public Build TriggerBuild(BuildTask build)
        {
            var xml = this.Serialize(build);
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, BuildQueueEndpoint);

            var response = this.requestExecutor.UploadData(url, Encoding.UTF8.GetBytes(xml));
            return this.Deserialize<Build>(response);
        }

        public Build GetQueuedBuildStatus(int queuedBuildId)
        {
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildQueueDetailsEndpoint, queuedBuildId));

            var response = this.requestExecutor.DownloadData(url);
            var responseStream = new MemoryStream(response);

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(XmlReader.Create(responseStream));
            return this.Deserialize<Build>(response);
        }

        public Build GetBuildStatus(int buildId)
        {
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildDetailsEndpoint, buildId));

            var response = this.requestExecutor.DownloadData(url);
            return this.Deserialize<Build>(response);
        }

        public Build CancelQueuedBuild(int queuedBuildId)
        {
            var xml = this.Serialize(new BuildCancelRequest());
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildQueueDetailsEndpoint, queuedBuildId));

            try
            {
                var response = this.requestExecutor.UploadData(url, Encoding.UTF8.GetBytes(xml));
                return this.Deserialize<Build>(response);
            }
            catch (WebException ex)
            {   // A possible cause for a 404 response is that the queued build was promoted to running build
                var httpResponse = ex.Response as HttpWebResponse;
                if (httpResponse == null || httpResponse.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
            }

            // As the queued build might be now running, attempt to get latest status and then cancel it
            var build = this.GetQueuedBuildStatus(queuedBuildId);
            return this.CancelBuild(build.Id);
        }

        public Build CancelBuild(int buildId)
        {
            var xml = this.Serialize(new BuildCancelRequest());
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildDetailsEndpoint, buildId));

            var response = this.requestExecutor.UploadData(url, Encoding.UTF8.GetBytes(xml));
            return this.Deserialize<Build>(response);
        }

        public IEnumerable<BuildTestOccurrence> GetBuildTestOccurrences(int buildId)
        {
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildTestOcurrencesListEndpoint, buildId));

            var response = this.requestExecutor.DownloadData(url);
            var testReferences = this.Deserialize<BuildTestOccurrenceReferences>(response);

            var occurrences = testReferences.References.Select(tor => this.GetBuildTestOccurrence(tor.RestApiReferenceUrl)).ToList();
            foreach (var occurrence in occurrences)
            {   // Trim test names
                occurrence.Name = occurrence.Name.Split('.').Last();
            }

            return occurrences;
        }

        public IEnumerable<Build> GetBuildReferences(string buildType, int sinceBuildNumber = 1)
        {
            var url = string.Format(BaseRestUrlTemplate, this.serverUrl, string.Format(BuildReferencesForTypeSinceLastBuildEndpoint, buildType, sinceBuildNumber));
            var response = this.requestExecutor.DownloadData(url);

            return this.Deserialize<BuildList>(response).Builds;
        }

        private BuildTestOccurrence GetBuildTestOccurrence(string referenceUrl)
        {
            var url = this.serverUrl + referenceUrl;

            var response = this.requestExecutor.DownloadData(url);

            return this.Deserialize<BuildTestOccurrence>(response);
        }

        private string Serialize<T>(T element)
        {
            var xmlBuilder = new StringBuilder();
            var settings = new XmlWriterSettings { Encoding = new UTF8Encoding(false), OmitXmlDeclaration = true };
            var writer = XmlWriter.Create(xmlBuilder, settings);
            var serializer = new XmlSerializer(typeof(T));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, element, namespaces);

            return xmlBuilder.ToString();
        }

        private T Deserialize<T>(byte[] response)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new MemoryStream(response));
        }
    }
}
