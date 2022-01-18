## STAIExtensions Host SignalR Client

This library contains a default .NET SignalR Client. It also contains a managed wrapper to ease development
of handling the connections and callbacks from and to the SignalR Server.

This package attempts to combine outgoing requests with incoming responses from the SignalR host
to ease development and complexities that arises from the SignalR hosting model. 

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)
<a href="https://trevormare.github.io/STAIExtensions/api/STAIExtensions.Host.SignalR.Client.html"><img src="https://img.shields.io/badge/Documentation-Help-informational?style=for-the-badge" /></a>

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.SignalR.Client?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.SignalR.Client/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.SignalR.Client?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.SignalR.Client/)


## Usage

```c#
var clientManager =
    new SignalRClientManaged(new SignalRClientManagedOptions("https://localhost:5001/STAIExtensionsHub",
        "Trevor Mare")
    {
        AuthBearerToken = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495", 
        UseDefaultAuthorization = true
    });
```

## Examples

For examples check the Examples directory. This package is used in
- Examples.SignalR.ConsoleClient

## Target Frameworks

- :heavy_check_mark: .NET Standard 2.1
- :heavy_check_mark: .NET Core 3.1
- :heavy_check_mark: .NET 5
- :heavy_check_mark: .NET 6

## Dependencies

- :heavy_check_mark: STAIExtensions.Abstractions
- :black_square_button: STAIExtensions.Core
- :black_square_button: STAIExtensions.Data.AzureDataExplorer
- :black_square_button: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client
- :heavy_check_mark: Microsoft.AspNetCore.SignalR.Client
