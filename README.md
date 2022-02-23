![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# STAIExtensions

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)

<a href="https://trevormare.github.io/STAIExtensions/api/STAIExtensions.Core.html"><img src="https://img.shields.io/badge/Documentation-Help-informational?style=for-the-badge" /></a>

## NOTE

It is with great sadness that I write this, but as of now I am no longer actively maintaining this project as some personal changes are forcing me to change direction. Hopefully one day I will be able to pick this up again.

## Concepts

### Telemetry Loader

For each of the unique data source where Application Insights is stored, there is a need for a Telemetry Loader
that understands how to connect and pull data from this data source. As part of the STAIExtensions package
a default telemetry loader is shipped with the knowledge to execute Kusto queries against the
standard Applications Insights store. This is the STAIExtensions.Data.AzureDataExplorer package.

The telemetry loader interface expects a method to be exposed to execute a query against a data source. In addition to this,
it should also expose a query builder factory that enables the data set object to build queries for on the standard models
included in application insights.

### Telemetry Loader Queries

The query definition is a entity that is defined specifically for each of the Telemetry Loader Types in the system. The
query factory will build a Query with an object property that is shared between the Telemetry Loader and the Query
The telemetry loader will then use this information to query the data source.

As an example:
- A data set needs to load all Exceptions from the data source. It calls the query factory to build an Exception Query with some predefined parameters.
- The returned query is executed on the Telemetry Loader instance and from the query entity it is able to retrieve all the information required to make a call to the data source.
- The telemetry loader deserializes the data into the known models and is returned to the Data Set.
- The data set is now the owner of the data and can persist it in any way it wants.

In the case of the default Azure Data Explorer, the query entity knows that this is a Kusto query and builds
the string to execute on the data source.

### Telemetry Loader Query Factory

This is a factory that is required by the Data Sets to build queries to be executed on the telemetry loader.

### Data Set

A data set instance is created with an instance of a Telemetry Loader. The data set has default logic to
update itself on an interval that is specified by it's options. When the refresh method is executed,
it reads the latest data from the telemetry loader and populates an aggregation of entities that it should
expose to the views.

To create your own Data Set, you need to implement either from the IDataSet interface
in the STAIExtensions.Abstractions project or you can inherit from the DataSet abstract class
provided in the STAIExtensions.Core.DataSets project. Take a look at the
STAIExtensions.Default project for implementation details.

A default data set (DataContractDataSet) is included in the STAIExtensions.Default nuget package. This data set
uses the Telemetry Loader to fetch a collection of the following entities.

- Availability
- BrowserTiming
- CustomEvents
- CustomMetrics
- Dependencies
- Exceptions
- PageViews
- PerformanceCounters
- Requests
- Traces

The default options specified for this data set is to load 10 000 into memory for each of the collections.
This can be changed when instantiating the data set.

***Note*** The default data set does not filter any data except for retrieving the most recent rows not yet in the collection.
It will be the responsibility of the view to filter and aggregate the results to accomodate the users selection.

Once the data retrieval is completed in the data set, it will notify each of it's attached views that new data is available.
The views can the update according to the user's parameters.

### Data View

A Data View is a definition that further propagates data into properties and is typically
fronted to users on a UI or to services in the backend. Due to there not being any restrictions
on the linking between Views and Data Sets, the view is responsible to check if the Data Set can
be used when updating. There is also a possibility that one view is attached to multiple
Data Sets of the same type. Depending on how the data is exposed, it might be required to keep track
and expose different properties for each Data Set that updated the view.

To create your own View, you need to inherit from the DataSetView abstract class
provided in the STAIExtensions.Core.Views project. Take a look at the
STAIExtensions.Default project for implementation details.

By default, there is a few Views included in the STAIExtensions.Default nuget package.


***NOTE*** This is still under construction!

### Data Set Collection / Data View Collection

The collections are implemented in the core to keep track and observe for any
changes in the entities. The collections management is built in by default, but if you need to extend it
make sure that you use the MediatR pattern. These collections also manages the linking between Views and Data Sets.

### Update Notifications

There are several libraries included that are included to assist in exposing the View data.

- Api Controllers (fetch data periodically)
- Grpc Host (push to user on View updated)
- SignalR Host (push to user on View updated)

## Workflow

### Server Host

The server host creates the Telemetry Clients and Data Sets. It can then use
one of the included libraries to expose the data to clients.

### Client

There are two client types of client libraries included, the first is a Grpc Client and
the second is a SignalR client.

- Grpc - .NET Grpc Client and wrapper. The protobuf file is also included in the Nuget package.
- SignalR - Javascript client wrapper that can be referenced from Github.
- SignalR - .NET client and wrapper implementation.

The typical workflow for the client packages are as follows. (Note that it might be different between packages).

1. Create the client instance with the host endpoint and a unique user identifier.
2. Attach to the View Updated events/callbacks on the managed client
3. Create a new View Instance you require for the user. You need to send the fully qualified type name for this.
4. On the returned View Id, link it to a Data Set for updates.

# License
Distributed under the MIT License. See [LICENSE](https://opensource.org/licenses/MIT) for more information.

## About Me

Trevor Maré - [trevorm336@gmail.com](mailto:trevorm336@gmail.com)

## My Other Projects
STDoodle - [STDoodle](https://github.com/TrevorMare/STDoodle)  
STGTour - [GTour](https://github.com/TrevorMare/STGTour)

Buy me a beer: [![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate?hosted_button_id=JTM723EPNE5N6)