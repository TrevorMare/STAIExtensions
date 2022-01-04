using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Host.Grpc;
using STAIExtensions.Host.Grpc.Client;

namespace STAIExtensions.Service;

public class Class1
{
    
    public static async Task Main()
    {
        
      

        try
        {
        
            Console.WriteLine("Initialising Grpc Connection");

            using var managedClient =
                new Host.Grpc.Client.GrpcClientManaged(new GrpcClientManagedOptions("https://localhost:44309", "ABC")
                {
                    UseDefaultAuthorization = true,
                    AuthBearerToken = "SameAsServerConfiguration"
                });
            // Attach to the Events
            Console.WriteLine("Attaching the dataset updates");
            
            managedClient.OnDataSetUpdated += (sender, id) =>
            {
                Console.WriteLine($"DataSet object with Id {id} has been updated");
            };

            managedClient.OnDataSetViewUpdated += (sender, id) =>
            {
                Console.WriteLine($"DataView object with Id {id} has been updated");
            };
            
            managedClient.OnDataSetViewUpdatedJson += (sender, e) =>
            {
                Console.WriteLine($"DataView object with Id {e.ViewId} has been updated to Json: {e.Payload}");
            };
          
            var availableDataSets = await managedClient.ListDataSetsAsync()!;

            if (availableDataSets != null && availableDataSets.Any())
            {
                Console.WriteLine("Creating View");

                var view = await managedClient.CreateViewAsync(
                    "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                
                Console.WriteLine($"View Created and returned with Id {view?.Id}. Attaching to dataset");

                if (view != null)
                {
                    var attachResult =
                        await managedClient.AttachViewToDatasetAsync(view.Id, availableDataSets.ToList()[0].DataSetId);
                    
                }
            }
            else
            {
                Console.WriteLine("No Datasets found");
            }


            
            Console.ReadLine();
            Console.WriteLine("Press any key to stop");

            Console.WriteLine("Closing Connections");
            Console.WriteLine("Done");

            Console.ReadLine();

        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.ReadLine();

        // var serviceCollection = new ServiceCollection();
        // ConfigureServices(serviceCollection);
        //
        // serviceCollection.UseSTAIExtensions();
        //
        // var serviceProvider = serviceCollection.BuildServiceProvider();
        //
        // var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions(null, null));
        // var dataset =
        //     new Core.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
        //
        // dataset.StartAutoRefresh(TimeSpan.FromSeconds(30));
        //
        // Console.ReadLine();
        //
        // dataset.StopAutoRefresh();


    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
    }
    
}