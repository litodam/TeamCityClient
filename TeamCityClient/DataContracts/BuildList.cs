namespace TeamCityClient.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "builds")]
    public class BuildList
    {
        [XmlElement(ElementName = "build")]
        public Build[] Builds { get; set; }
    }
}