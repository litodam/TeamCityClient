namespace TeamCityClient.Console
{
    using System;
    using System.Configuration;
    using System.Linq;
    using FluentTc;
    using Serilog;

    public class Program
    {
        private static IConnectedTc TeamCityClient;

        public static void Main(string[] args)
        {
            string teamCityServerUrl = ConfigurationManager.AppSettings["TeamCityServerUrl"];
            string teamCityUsername = ConfigurationManager.AppSettings["TeamCityUsername"];
            string teamCityPassword = ConfigurationManager.AppSettings["TeamCityPassword"];

            TeamCityClient = new RemoteTc().Connect(c => c.ToHost(teamCityServerUrl).AsUser(teamCityUsername, teamCityPassword));

            string projectId = "ACOM";

            ConfigureLogger();

            //FindAllConfigurationsWithCertainParameterValue("8ae04749-b4da-4891-bb2d-d8efa6c490af", projectId);
            //FindAllConfigurationsWithCertainParameterValue("Buildr.CreateBranchWebSite.AccountUserName", projectId, true);            
            //FindAllConfigurationsWithCertainParameterValue("Buildr.CreateBranchWebSite.AccountUserName", useParameKeyInsteadOfValue: true);    

            FindAllConfigurationsWithACertainStepName("StyleCop");

            Console.WriteLine("{0}Press any key to exit..", Environment.NewLine);
            Console.ReadKey();
        }

        private static void ConfigureLogger()
        {
            var log = new LoggerConfiguration()
                        .WriteTo.ColoredConsole()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .CreateLogger();

            Log.Logger = log;
        }

        private static void FindAllConfigurationsWithCertainParameterValue(string paramValue, string projectId = null, bool useParameKeyInsteadOfValue = false)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = "_Root";
            }

            var configurations = TeamCityClient.GetBuildConfigurations(bc => bc.ProjectRecursively(p => p.Id(projectId)));

            int count = 0;
            foreach (var config in configurations)
            {
                var configDetails = TeamCityClient.GetBuildConfiguration(c => c.Id(config.Id));
                
                var parameters = configDetails.Parameters.Property.Where(p => (!useParameKeyInsteadOfValue && p.Value.Contains(paramValue)) || (useParameKeyInsteadOfValue && p.Name.Contains(paramValue)));

                if (parameters.Any())
                {
                    count++;
                    string message = "Parameter value {0} found in Configuration {1} (Id: {2}). Parameters:{3:l}";
                    parameters.ToList().ForEach(p => message = string.Concat(message, string.Format("\t{0}: {1}{2}", p.Name, p.Value, Environment.NewLine)));

                    Log.Information(message, paramValue, config.Name, config.Id, Environment.NewLine);
                }
            }

            Log.Information("Parameter value {0} found in {1} configuration(s) out of {2} under project {3}.", paramValue, count, configurations.Count, projectId);
        }

        private static void FindAllConfigurationsWithACertainStepName(string stepName, string projectId = null)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                projectId = "_Root";
            }

            var configurations = TeamCityClient.GetBuildConfigurations(bc => bc.ProjectRecursively(p => p.Id(projectId)));

            int count = 0;
            foreach (var config in configurations)
            {
                var configDetails = TeamCityClient.GetBuildConfiguration(c => c.Id(config.Id));

                var steps = configDetails.Steps.Step.Where(p => p.Name.Contains(stepName) || p.Name.ToLowerInvariant().Contains(stepName.ToLowerInvariant()));

                if (steps.Any())
                {
                    count++;
                    string message = "Step with name containing '{0}' found in Configuration {1} (Id: {2}).";                   

                    Log.Information(message, stepName, config.Name, config.Id, Environment.NewLine);
                }
            }

            Log.Information("Step with name containing '{0}' found in {1} configuration(s) out of {2} under project {3}.", stepName, count, configurations.Count, projectId);
        }
    }
}
