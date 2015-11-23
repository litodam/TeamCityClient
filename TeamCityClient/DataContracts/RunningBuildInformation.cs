namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    public class RunningBuildInformation
    {
        [XmlAttribute(AttributeName = "CurrentStageText")]
        public string CurrentStageStatusDescription { get; set; }
    }
}
