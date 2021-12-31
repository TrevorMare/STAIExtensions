## STAIExtensions Abstractions

At a top level, this library defines the interfaces for queries, datasets, dataset views, AI data contracts models, the managing collections of the datasets and views 
and lastly the CQRS pattern to update the entities. The rest of the project relies heavily on this
project for the dataflow and shared structures.

## Nuget

```http request
https://nuget.org/TODO
```

### Collections
Defines the interfaces for the collection management of views and data sets. This also includes an interface for
a list with a fixed size.

### Common
Additional information classes returned from the abstraction library on registered data set and data set view types

### CQRS
General CQRS pattern that will interact with the collections, datasets and views. The **MediatR** package is used to assist with the pattern. 

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

## Dependencies

- MediatR
- MediatR.Extensions.Microsoft.DependencyInjection
- System.ComponentModel.Annotations
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.DependencyInjection