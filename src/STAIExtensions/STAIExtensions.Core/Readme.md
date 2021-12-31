## STAIExtensions Core

This library contains the default implementation of the Dataset and Dataset View Collections
as well as the startup classes to register the required DI Objects.

## Nuget

```http request
https://nuget.org/TODO
```

### Collections
Default implementations of the Dataset and Dataset View Collections and the Fixed Lists.

### DataSets
Abstract implementation of the default dataset object required by the project

### Views
Abstract implementation of the default dataset view object required by the project

### Other

StartupExtensions - This is the entry for the hosting application to register the Dependency Injection Services required by this project.

```c#
using STAIExtensions.Core;

...

builder.Services.UseSTAIExtensions();

...

```

## Dependencies

- System.ServiceModel.Primitives