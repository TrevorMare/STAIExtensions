## STAIExtensions Host Grpc Client

This library contains a default .NET Protobuf Client. It also contains a managed wrapper to ease development
of handling the connections and callbacks from and to the Grpc Server.

## Target Frameworks

- .NET Standard 2.1
- .NET Core 3.1
- .NET 5
- .NET 6

## Nuget

```http request
https://nuget.org/TODO
```

## Usage

To use the library in a .NET project, install the package from Nuget. Once installed, either the managed client
or the built in generated Protobuf client can be used as both are exposed.

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

| Method                     | Async Available | Description |
|:---------------------------|:---------------:|:------------|
| AttachViewToDataset        |       yes       |             |
| CreateView                 |       yes       |             |
| DetachViewFromDataset      |       yes       |             |
| GetRegisteredViews         |       yes       |             |
| GetView                    |       yes       |             |
| GetViewJson                |       yes       |             |
| ListDataSets               |       yes       |             |
| OnDataSetUpdated           |       yes       |             |
| OnDataViewUpdated          |       yes       |             |
| RemoveView                 |       yes       |             |
| SetViewAutoRefreshDisabled |       yes       |             |
| SetViewAutoRefreshEnabled  |       yes       |             |
| SetViewParameters          |       yes       |             |





## Dependencies

- Google.Protobuf
- Grpc.Net.Client
- Grp.Tools (Internal)
