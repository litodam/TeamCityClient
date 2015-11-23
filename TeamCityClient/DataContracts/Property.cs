namespace TeamCityClient.DataContracts
{
    using System.Xml.Serialization;

    public class Property
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
