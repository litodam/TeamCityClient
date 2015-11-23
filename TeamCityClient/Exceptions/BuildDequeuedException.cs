namespace TeamCityClient.Exceptions
{
    using System;
    using DataContracts;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable", Justification = "Internal Exception")]
    public class BuildDequeuedException : Exception
    {
        public BuildDequeuedException()
            : base("Build was dequeued")
        {
        }

        public BuildDequeuedException(string message)
            : base(message)
        {
        }

        public BuildDequeuedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public Build BuildStatus { get; set; }
    }
}
