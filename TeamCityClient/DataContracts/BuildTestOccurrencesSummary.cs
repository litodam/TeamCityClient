namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    public class BuildTestOccurrencesSummary
    {
        [XmlAttribute(AttributeName = "count")]
        public int TestCount { get; set; }

        [XmlAttribute(AttributeName = "ignored")]
        public int IgnoredTestsCount { get; set; }
    }
}