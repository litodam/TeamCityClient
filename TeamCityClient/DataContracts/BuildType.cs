namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    public class BuildType
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "href")]
        public string RestDetailsUrl { get; set; }

        [XmlAttribute(AttributeName = "projectName")]
        public string ProjectName { get; set; }

        [XmlAttribute(AttributeName = "projectId")]
        public string ProjectId { get; set; }

        [XmlAttribute(AttributeName = "webUrl")]
        public string DetailsPortalUrl { get; set; }
    }
}
