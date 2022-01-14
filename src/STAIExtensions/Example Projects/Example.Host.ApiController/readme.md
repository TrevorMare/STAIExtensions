![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

## Examples: Host Api Controller

This is an example WebApi project that hosts the injected Api controllers 
from the STAIExtensions.Host.Api project. To interact with this project it is recommended
to use a tool like Postman as the Authorization is not built into the swagger definition. You
can also disable the Authorization and call it from the Swagger UI.

The Api Controller option is the most basic of fronting the telemetry data and 
when SignalR or Grpc is not an option, this library will allow for a periodic pull of the Views.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)


## Disabled Authorization

```c#
builder.Services.UseSTAIExtensionsApiHost(() => new ApiOptions() { UseAuthorization = false });
```

## Enabled Authorization

When the Authorization is enabled, the controllers expect a header with the name x-api-key to be passed along with the request. The value of this header should match
one of the keys defined in the AllowedApiKeys option.

```c#
builder.Services.UseSTAIExtensionsApiHost(() => new ApiOptions() 
{ 
    UseAuthorization = true,
    HeaderName = "x-api-key",
    AllowedApiKeys = new List<string>() { "MyApiKey" }} 
});
```

## Data Source
This example connects to a default hosted Application Insights instance that is fed data 
from all the example projects. It might not have a lot of data present initially but if you run the examples
for a while it should generate enough for the example. To connect it to another instance, update the AppSettings.json file to point to another instance.

The DataSets are created in a background Hosted Service that runs as long as the Api process is running.

***WARNING***: The sample Application Insights instance can be removed at any time.

## Required packages

- :heavy_check_mark: STAIExtensions.Core
- :heavy_check_mark: STAIExtensions.Data.AzureDataExplorer
- :heavy_check_mark: STAIExtensions.Default
- :heavy_check_mark: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client