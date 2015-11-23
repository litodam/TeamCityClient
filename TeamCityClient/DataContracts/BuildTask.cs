namespace TeamCityClient.DataContracts
{
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "build")]
    public class BuildTask
    {
        [XmlAttribute(AttributeName = "personal")]
        public bool Personal { get; set; }

        [XmlElement(ElementName = "buildType")]
        public BuildType BuildType { get; set; }

        [XmlElement(ElementName = "commentText")]
        public string Comment { get; set; }

        [XmlArray(ElementName = "properties")]
        [XmlArrayItem(ElementName = "property")]
        public Property[] Properties { get; set; }

        public string this[string index]
        {
            get 
            {
                var property = this.Properties.SingleOrDefault(p => p.Name == index);

                if (property != null)
                {
                    return property.Value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
