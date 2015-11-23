namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "buildCancelRequest")]
    public class BuildCancelRequest
    {
        private const string DefaultComment = "Cancelation requested by Azure Store tools portal.";

        public BuildCancelRequest()
        {
            this.Comment = DefaultComment;
            this.ReaddToQueue = false;
        }

        [XmlElement(ElementName = "comment")]
        public string Comment { get; set; }

        [XmlElement(ElementName = "readdIntoQueue")]
        public bool ReaddToQueue { get; set; }
    }
}