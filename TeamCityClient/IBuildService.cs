namespace TeamCityClient
{
    using System.Collections.Generic;
    using DataContracts;

    public interface IBuildService
    {
        Build CancelBuild(int buildId);

        Build CancelQueuedBuild(int queuedBuildId);

        Build GetBuildStatus(int buildId);

        Build GetQueuedBuildStatus(int queuedBuildId);
        
        Build TriggerBuild(BuildTask build);

        IEnumerable<BuildTestOccurrence> GetBuildTestOccurrences(int buildId);

        IEnumerable<Build> GetBuildReferences(string buildType, int sinceBuildNumber = 0);
    }
}
