namespace TeamCityClient.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "testOccurrence")]
    public class BuildTestOccurrence
    {
        private static readonly Regex TestOccurrenceIdMatch = new Regex(@"id\:(\d*)");

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "href")]
        public string RestApiReferenceUrl { get; set; }

        [XmlElement(ElementName = "details")]
        public string TestLog { get; set; }

        public int TestOccurrenceId
        {
            get
            {
                var id = TestOccurrenceIdMatch.Match(this.Id).Groups[1].Value;
                return int.Parse(id);
            }
        }
    }
}