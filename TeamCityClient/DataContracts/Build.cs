namespace TeamCityClient.DataContracts
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "build")]
    public class Build
    {
        private const string DateFormat = "yyyyMMddTHHmmsszzz";

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "taskId")]
        public int QueuedId { get; set; }

        [XmlAttribute(AttributeName = "webUrl")]
        public string BuildStatusPortalUrl { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }

        [XmlElement(ElementName = "running-info")]
        public RunningBuildInformation RunningBuildInformation { get; set; }

        [XmlElement(ElementName = "statusText")]
        public string StatusDescription { get; set; }

        [XmlElement(ElementName = "testOccurrences")]
        public BuildTestOccurrencesSummary TestOcurrencesSummary { get; set; }

        [XmlIgnore]
        public DateTime? FinishDate { get; set; }

        [XmlIgnore]
        public bool IsRunning
        {
            get
            {
                return this.RunningBuildInformation != null;
            }
        }

        [XmlIgnore]
        public int? BuildNumber { get; set; }

        #region Proxy fields for proper data conversion

        [XmlElement(ElementName = "finishDate")]
        public string FinishDateProxy
        {
            get
            {
                return this.FinishDate.HasValue ? this.FinishDate.Value.ToString(DateFormat) : null;
            }

            set
            {
                var provider = CultureInfo.InvariantCulture;
                this.FinishDate = string.IsNullOrEmpty(value) ? (DateTime?)null : DateTime.ParseExact(value, DateFormat, provider);
            }
        }

        [XmlAttribute(AttributeName = "number")]
        public string BuildNumberProxy
        {
            get
            {
                return this.BuildNumber.HasValue ? this.BuildNumber.Value.ToString() : null;
            }

            set
            {
                int outValue = 0;
                this.BuildNumber = int.TryParse(value, out outValue) ? outValue : (int?)null;
            }
        }

        #endregion
    }
}
