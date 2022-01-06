## STAIExtensions Host Grpc Client

This library contains a default .NET Protobuf Client. It also contains a managed wrapper to ease development
of handling the connections and callbacks from and to the Grpc Server.



[![.NET](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/dotnet.yml?style=for-the-badge)](https://github.com/TrevorMare/STAIExtensions/actions/workflows/dotnet.yml)

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.Grpc.Client?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Grpc.Client/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.Grpc.Client?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Grpc.Client/)


[https://www.nuget.org/packages/STAIExtensions.Host.Grpc.Client/](https://www.nuget.org/packages/STAIExtensions.Host.Grpc.Client/)

## Usage

To use the library in a .NET project, install the package from Nuget. Once installed, either the managed client
or the built in generated Protobuf client can be used as both are exposed.

The one parameter to take note of is the Owner Id parameter used throughout the lifetime of this library. 
The object instantiating this class should create a unique value for the owner (or User) to be passed to the backend.
This will ensure that the callbacks when views are updated are returned to the correct instance of the Grpc client. 
A suggested approach is to either generate a new Guid for the lifespan of the connection or use a user name for this value.

It should also be noted that when a user name is used for the owner and a view's parameters are set this will update the view globally. 
These changes will propagate to all other retrievals like the API and SignalR GetView data.

## General Flow

For the general flow of constructing views and attaching it to datasets, see the main project readme documentation.

### Generated Client

For more control over the process, the generated client can be used to connect to the host Grpc Server.
The generated client exposes both Async and Sync calls.

#### Example Code: 

```c#
using Grpc.Net.Client;

...

private STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient _client;

public void Connect()
{
    var channel = GrpcChannel.ForAddress("https://localhost:1234");
    _client = new STAIExtensionsGrpcService.STAIExtensionsGrpcServiceClient(channel);
}

public void GetView(string viewId, string ownerId, CancellationToken cancellationToken = default)
{
    var response = this._client.GetView(new GetViewRequest()
    {
        OwnerId = ownerId,
        ViewId = viewId
    }, null, null, cancellationToken);
    // Do something with the response        
}
...

```

#### Generated Client Methods:

| Method                     | Async Available | Description                                                                                                                                                      |
|:---------------------------|:---------------:|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AttachViewToDataset        |       yes       | Attaches a view to a dataset for updates                                                                                                                         |
| CreateView                 |       yes       | Creates a new view with the specified type name                                                                                                                  |
| DetachViewFromDataset      |       yes       | Detaches the view from the dataset                                                                                                                               |
| GetMyViews                 |       yes       | Get the views linked to the owner Id                                                                                                                             |
| GetRegisteredViews         |       yes       | Load all the types of views that can be created                                                                                                                  |
| GetView                    |       yes       | Gets the view in it's current state. Due to the message type only the common properties are returned                                                             |
| GetViewJson                |       yes       | Gets the view in a Json format with all defined properties                                                                                                       |
| ListDataSets               |       yes       | Lists the currently running datasets that views can be attached to                                                                                               |
| OnDataSetUpdated           |       yes       | Callback stream that notifies the client that a dataset has updated                                                                                              |
| OnDataViewUpdated          |       yes       | Callback stream that notifies the client that a view belonging to the owner has been updated                                                                     |
| RegisterConnectionState    |       yes       | Callback stream to monitor the streaming connection states. The server will stream the server time in UTC every second                                           |
| RemoveView                 |       yes       | Removes a view from all datasets and disposes it                                                                                                                 |
| SetViewAutoRefreshDisabled |       yes       | Pauses the view updates on a view                                                                                                                                |
| SetViewAutoRefreshEnabled  |       yes       | Resumes the view updates on a view                                                                                                                               |
| SetViewParameters          |       yes       | Sets the view parameters. The allowed parameters can be found on the view. The Rpc call takes a value in the format of a Dictionary<string, object> Json payload |


### Managed Client

The managed client is a wrapper around the common functions and is there to assist with the base functions and setup.
As part of the options it also allows the full option set of the GrpcChannelOptions to be passed along. Another option
exposed is the UseHttp2UnencryptedSupport that adds legacy support for Grpc.

Additionally to the basic setup and configuration, this managed client allows for the default authorization 
to be injected as part of the options. This will use a Bearer token that will be sent across in the header metadata
and validated against the server. The server host and the client bearer token must be the same.

The managed client will also automatically watch the streaming connection states and attempt to reconnect the channel and client
when a connection drop is noticed. This might work fine in some cases, but when the hosting application
restarted, the Views that were created during this session will not exist and have to be recreated.

#### Example Code:

***Note***: The example below just waits for the connection to establish before continuing with the rest of the operations. A better practice would
be to attach an event to the OnConnectionStateChanged event and drive the process from there.

```c#

public async Task SetupManagedClient()
{
    using var managedClient =
        new Host.Grpc.Client.GrpcClientManaged(new GrpcClientManagedOptions("https://localhost:44309", "ABC")
        {
            UseDefaultAuthorization = true,
            AuthBearerToken = "SameAsServerConfiguration"
        });
    
    // Wait for the connection
    while (managedClient.ConnectionState != ConnectionState.Connected)
    {
        await Task.Delay(200);
    }
    
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
    
    var view = await managedClient.CreateViewAsync(
        "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
    ....
    
}

```

#### Managed Client Methods:

All the calls via the managed client are Asynchronous. The callback stream channels are handled in the managed client and will raise the appropriate events that can be attached to.

| Method                          | Description                                                                                                                                                      |
|:--------------------------------|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AttachViewToDatasetAsync        | Attaches a view to a dataset for updates                                                                                                                         |
| CreateViewAsync                 | Creates a new view with the specified type name                                                                                                                  |
| DetachViewFromDatasetAsync      | Detaches the view from the dataset                                                                                                                               |
| GetMyViewsAsync                 | Get the views linked to the owner Id                                                                                                                             |
| GetRegisteredViewsAsync         | Load all the types of views that can be created                                                                                                                  |
| GetViewAsync                    | Gets the view in it's current state. Due to the message type only the common properties are returned                                                             |
| GetViewJsonAsync                | Gets the view in a Json format with all defined properties                                                                                                       |
| ListDataSetsAsync               | Lists the currently running datasets that views can be attached to                                                                                               |
| RemoveViewAsync                 | Removes a view from all datasets and disposes it                                                                                                                 |
| SetViewAutoRefreshDisabledAsync | Pauses the view updates on a view                                                                                                                                |
| SetViewAutoRefreshEnabledAsync  | Resumes the view updates on a view                                                                                                                               |
| SetViewParametersAsync          | Sets the view parameters. The allowed parameters can be found on the view. The Rpc call takes a value in the format of a Dictionary<string, object> Json payload |

## Target Frameworks

- .NET Standard 2.1
- .NET Core 3.1
- .NET 5
- .NET 6

## Dependencies

- Google.Protobuf
- Grpc.Net.Client
- Grp.Tools (Internal)
- System.Text.Json
