## STAIExtensions Core

This library contains the default implementation of the DataSet Collections, DataSet View Collections
and the abstract DataSet and DataSetView objects

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Core?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Core/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Core?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Core/)

[https://www.nuget.org/packages/STAIExtensions.Core/](https://www.nuget.org/packages/Core/)

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

- .NET Standard 2.1
- .NET Core 3.1
- .NET 5
- .NET 6

## Dependencies

- System.ServiceModel.Primitives
- STAIExtensions.Abstractions