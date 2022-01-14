using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Host.Grpc.Client;
using STAIExtensions.Host.Grpc.Client.Common;

namespace Examples.Grpc.Client;

public static class Program
{
    
    public static void Main()
    {
        try
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<GrpcClientManaged>>();
        
            Console.WriteLine("Initialising Grpc Connection");

            using var managedClient =
                new GrpcClientManaged(new GrpcClientManagedOptions("https://localhost:5001", "Trevor Mare")
                    {
                        AuthBearerToken = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666",
                        UseDefaultAuthorization = true
                    }
                    ,logger);
            
            // Attach to the connection state event
            managedClient.OnConnectionStateChanged += async (sender, state) =>
            {
                if (state == ConnectionState.Connected)
                {
                    // Setup the views for the listener
                    await CreateViewsAndAttach(managedClient);
                }
            };

            // Attach to the updated events
            managedClient.OnDataSetUpdated += (sender, id) =>
            {
                Console.WriteLine($"DataSet object with Id {id} has been updated");
            };

            managedClient.OnDataSetViewUpdated += (sender, id) =>
            {
                Console.WriteLine($"View object with Id {id} has been updated");
            };
            
            managedClient.OnDataSetViewUpdatedJson += (sender, e) =>
            {
                Console.WriteLine($"View {e.ViewId} has been updated with body : {e.Payload}");
            };

            Console.WriteLine("Press any key to stop");
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
    }

    private static async Task CreateViewsAndAttach(GrpcClientManaged managedClient)
    {
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
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Trace);
    }
    
}