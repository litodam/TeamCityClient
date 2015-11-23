namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "testOccurrences")]
    public class BuildTestOccurrenceReferences : BuildTestOccurrencesSummary
    {
        [XmlElement(ElementName = "testOccurrence")]
        public BuildTestOccurrence[] References { get; set; }
    }
}