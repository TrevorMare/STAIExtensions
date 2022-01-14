![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# Examples: Grpc Host Console

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)

This is an example project that hosts the Grpc Server in a console application. By default it
it spins up a Kestrel server with on the https://localhost:5001 endpoint. You can use the Examples.Grpc.Client
project to connect to this instance of the service. 

If you change the default Authorization details here, you would also need to change the client to reflect the same changes.

## Disabled Authorization

```c#
    // Create the required services for the Grpc Channels and Authorization
    services.UseSTAIGrpc(new GrpcHostOptions(false, null));
```

## Enabled Authorization
```c#
   // Create the required services for the Grpc Channels and Authorization
   services.UseSTAIGrpc(new GrpcHostOptions(true, config["GrpcAuthorizationToken"]));
```

## Data Source
This example connects to a default hosted Application Insights instance that is fed data
from all the example projects. It might not have a lot of data present initially but if you run the examples
for a while it should generate enough for the example. To connect it to another instance, update the AppSettings.json file to point to another instance.

The DataSets are created in a background Hosted Service that runs as long as the process is running.

***WARNING***: The sample Application Insights instance can be removed at any time.

If you want to change the Data Source to read the telemetry data from your Application Insights instance,
check the documentation [here](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/STAIExtensions.Data.AzureDataExplorer)
to download and generate keys for access.

## Required packages

- :black_square_button: STAIExtensions.Core
- :heavy_check_mark: STAIExtensions.Data.AzureDataExplorer
- :heavy_check_mark: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :heavy_check_mark: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client