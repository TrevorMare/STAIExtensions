![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# STAIExtensions Abstractions

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/badge/Documentation-Help-lightgrey?style=for-the-badge)



## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Abstractions?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Abstractions/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Abstractions?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Abstractions/)

This library is the root that defines the interfaces for queries, datasets, dataset views, Application Insights data contract models, the collection management interfaces 
and lastly the CQRS pattern to interact with the system. The rest of the projects rely heavily on this
project for the dataflow and shared structures.

Additionally it defines the Mediator pattern that the implemented interfaces use, the MediatR package 
is used to keep the collections in sync and registration for UI interactions. All projects
using this library and the core library should use the provided MediatR commands and queries and not change the collections
directly. On startup the MediatR services needs to be registered. If you are using this library as a direct reference without the Core library, you would need
to register the services by calling the **DependencyExtensions.UseSTAIExtensionsAbstractions** method.

```c# 
    services.UseSTAIExtensionsAbstractions();
```

If you are implicitly loading this package via the STAIExtions.Core package, this
will be handled internally.

### Collections
Defines the interfaces for the collection management of views and data sets. This also includes an interface for
a list with a fixed size.

### Common
Additional information classes returned from the abstraction library on registered data set and data set view types

### CQRS
General CQRS pattern that will interact with the collections, datasets and views. The **MediatR** package is used to assist with the pattern.
To get an instance of the MediatR object, it can simply be injected into a class or a helper method is also available to retrieve the instance.

Example: Retrieving the MediatR instance
```c#
    var mediatR = STAIExtensions.Abstractions.DependencyExtensions.Mediator;
```

### Data
Defines the interfaces for the telemetry loader and the dataset.

### Data Contracts
Defines the POCO objects that are returned by the telemetry loader as defined by the standard application insights data contracts. 

### Queries
Defines the interfaces, abstracts for the data contract query as well as the query factory used by the telemetry loader interface. 

### Views
Defines the interface for the data set view object and parameter descriptors.

### Other

DependencyExtensions - Due to current limitations with the dependency injection used, this class is implemented around an Anti-Pattern
of persisting the IServiceCollection to retrieve the singleton collections.

This class is automatically instantiated by the STAIExtensions.Core project and does not need to be called manually. 

## Target Frameworks

- :heavy_check_mark: .NET Standard 2.1
- :heavy_check_mark: .NET Core 3.1
- :heavy_check_mark: .NET 5
- :heavy_check_mark: .NET 6

## Dependencies

- :heavy_check_mark: MediatR
- :heavy_check_mark: MediatR.Extensions.Microsoft.DependencyInjection
- :heavy_check_mark: System.ComponentModel.Annotations
- :heavy_check_mark: Microsoft.Extensions.Logging.Abstractions
- :heavy_check_mark: Microsoft.Extensions.DependencyInjection.Abstractions
- :heavy_check_mark: Microsoft.Extensions.DependencyInjection
- :heavy_check_mark: Microsoft.ApplicationInsights
- :heavy_check_mark: System.Diagnostics.DiagnosticsSource
- :heavy_check_mark: System.Text.Json