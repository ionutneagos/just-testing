namespace BootCamp.Inventory.Api
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            var executioncontextoptions = builder.Services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;
            var currentDirectory = executioncontextoptions.AppDirectory;

            var config = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                   .AddEnvironmentVariables()
                                                   .Build();
            //TO DO
            //Add CosmosDb. This verifies database and collections existence.
        }
    }
}
