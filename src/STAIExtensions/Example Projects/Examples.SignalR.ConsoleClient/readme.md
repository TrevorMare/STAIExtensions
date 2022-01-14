![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# Examples: SignalR Client (c# Managed)

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)

This is an example project that connects to the SignalR host application and generates log outputs as an example.
It uses the provided managed c# client wrapper to communicate with the SignalR host.
It connects to the hosting SignalR server (Be sure to start the Examples.Host.ApiFull project before running this) and
outputs a log to the UI as the events occur.

For more information on the usage of this package, read the documentation provided on
[STAIExtensions.Host.SignalR.Client](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/STAIExtensions.Host.SignalR.Client)


## Usage

The managed client is created with the default Authorization options and keys, be 
sure to update it if your keys differ.

```c#
    var clientManager =
        new SignalRClientManaged(new SignalRClientManagedOptions("https://localhost:5001/STAIExtensionsHub",
            "Trevor Mare")
        {
            AuthBearerToken = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495", 
            UseDefaultAuthorization = true
        });
```

## Required packages

- :black_square_button: STAIExtensions.Core
- :black_square_button: STAIExtensions.Data.AzureDataExplorer
- :black_square_button: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :heavy_check_mark: STAIExtensions.Host.SignalR.Client