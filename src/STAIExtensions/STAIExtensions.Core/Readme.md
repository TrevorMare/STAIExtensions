## STAIExtensions Core

This library contains the default implementation of the DataSet Collections, DataSet View Collections
and the abstract DataSet and DataSetView objects

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)
<a href="https://trevormare.github.io/STAIExtensions/api/STAIExtensions.Core.html"><img src="https://img.shields.io/badge/Documentation-Help-informational?style=for-the-badge" /></a>


## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Core?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Core/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Core?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Core/)

### Collections
Default implementations of the Dataset and Dataset View Collections and the Fixed Lists.

### DataSets
Abstract implementation of the default dataset object required by the project

### Views
Abstract implementation of the default dataset view object required by the project

### Other

StartupExtensions - This is the entry for the hosting application to register the Dependency Injection Services required by this project. This will in turn also call the 
Abstractions project startup extensions and register the needed services.

```c#
using STAIExtensions.Core;

...

builder.Services.UseSTAIExtensions();

...

```
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
- :heavy_check_mark: System.ServiceModel.Primitives