## STAIExtensions Host Grpc

This library contains the .NET Protobuf Grpc Service. It can be hosted in both console applications
and in ASPNet Web projects. For example usage, check the Examples folder.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)
<a href="https://trevormare.github.io/STAIExtensions/api/STAIExtensions.Host.Grpc.html"><img src="https://img.shields.io/badge/Documentation-Help-informational?style=for-the-badge" /></a>

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.Grpc?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Grpc/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.Grpc?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Grpc/)

## Protobuf

The copy of the protobuf file is included as content in the Nuget package and 
can be retrieved via a the Nuget Package Explorer.

## Usage

```c#

using STAIExtensions.Host.Grpc;

...
// *********************
// Register the Grpc Service here
// *********************
builder.Services.UseSTAIGrpc(new GrpcHostOptions());

var app = builder.Build();

...

// *********************
// Map the Grpc Controllers here after routing and Authorization
// *********************
app.MapSTAIGrpc(app.Environment.IsDevelopment());
...

```

## Security Options

By default the Grpc Service Host will use the default Authorization setup in the library

- Key Value: 598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666

These values can be set when registering the Service Host.

```c#

var grpcOptions = new GrpcHostOptions(true, builder.Configuration["GrpcAuthorizationToken"]);
builder.Services.UseSTAIGrpc(grpcOptions);

```

## Examples

For examples check the Examples directory. This package is used in
- Examples.Grpc.Host.Console
- Examples.Host.ApiFull


## Target Frameworks

- :black_square_button: .NET Standard 2.1
- :heavy_check_mark: .NET Core 3.1
- :heavy_check_mark: .NET 5
- :heavy_check_mark: .NET 6

## Dependencies

- :black_square_button: STAIExtensions.Abstractions
- :heavy_check_mark: STAIExtensions.Core
- :black_square_button: STAIExtensions.Data.AzureDataExplorer
- :black_square_button: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client
- :heavy_check_mark: Grpc.AspNetCore
- :heavy_check_mark: Grpc.AspNetCore.Server.Reflection
- :heavy_check_mark: Grp.Tools (Internal)



