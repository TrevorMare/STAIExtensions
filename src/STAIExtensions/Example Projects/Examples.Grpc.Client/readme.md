![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# Examples: Grpc Client
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)


This is an example project that connects to a hosted Grpc Server instance with the managed client. It is intended to work alongside
the Examples.Grpc.Host.Console or Examples.Host.ApiFull projects. The default port that it connects to is on https://localhost:5001.

To run this example, first start up one of the Hosting applications and make sure that
the port for the host is 5001. Next build and run this project, the console will log the informational outputs.


## Authorization Keys
The demo sites all use the same Authorization Keys to allow for the connections. If you change the host
configuration, you will need to update the configuration here as well.

## Usage

```c# 
  using var managedClient =
    new GrpcClientManaged(new GrpcClientManagedOptions("https://localhost:5001", "Your Name")
        {
            AuthBearerToken = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666",
            UseDefaultAuthorization = true
        }
        , logger);
```

## Required packages

- :black_square_button: STAIExtensions.Core
- :black_square_button: STAIExtensions.Data.AzureDataExplorer
- :black_square_button: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :heavy_check_mark: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client