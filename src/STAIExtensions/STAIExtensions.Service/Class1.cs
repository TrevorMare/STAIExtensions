using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Host.Grpc;

namespace STAIExtensions.Service;

public class Class1
{
    
    public static void Main()
    {
        
      

        try
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            
            Console.WriteLine("Initialising Grpc Connection");
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:44309");
            var client = new STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient(channel);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            
            
            Console.WriteLine("Attaching the dataset updates");
            // Attach the callbacks
            Task.Run(async () =>
            {
                var dataSetUpdated = client.OnDataSetUpdated(new NoParameters(), null, null, cancellationToken);
                while (await dataSetUpdated.ResponseStream.MoveNext(cancellationToken))
                {
                    Console.WriteLine("Dataset updated with Id: " + dataSetUpdated.ResponseStream.Current.DataSetId);
                }
            }, cancellationToken);
            
            
            Task.Run(async () =>
            {
                var viewUpdated = client.OnDataViewUpdated(new OnDataSetViewUpdatedRequest() { OwnerId = "ABC"}, null, null, cancellationToken);
                while (await viewUpdated.ResponseStream.MoveNext(cancellationToken))
                {
                    Console.WriteLine("View Id updated with Id: " + viewUpdated.ResponseStream.Current.ViewId);
                }
            }, cancellationToken);
            
           
            // Get the datasets 
            Console.WriteLine("Attaching the dataset updates");
            var availableDataSets = client.ListDataSets(new NoParameters());

            if (availableDataSets.Items.Count > 0)
            {
                Console.WriteLine("Creating View");
                var view = client.CreateView(new CreateViewRequest()
                {
                    OwnerId = "Abc",
                    ViewType = "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                }, null, null, cancellationToken);

                Console.WriteLine($"Attaching View with Id {view.Id} to DataSet {availableDataSets.Items[0].DataSetId}");
                client.AttachViewToDataset(new AttachViewToDatasetRequest()
                {
                    DataSetId = availableDataSets.Items[0].DataSetId,
                    OwnerId = "Abc",
                    ViewId = view.Id
                });
            }
            else
            {
                Console.WriteLine("No Datasets found");
            }

            Console.ReadLine();
            Console.WriteLine("Press any key to stop");
            cancellationTokenSource.Cancel();

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