## STAIExtensions Data AzureDataExplorer

### Overview
This library is included to enable Application Insights to be read via the Azure Data Explorer Api. 
In most cases this should be enough to get going. This package exposes abstract queries
that can be implemented and load custom KUSTO query data from the source. By default
it is able to populate all the models found in Application Insights at this point in time.

## Nuget

```http request
https://nuget.org/TODO
```

## Usage

To use this project to populate datasets, create a new instance of the telemetry loader
with the Api Key and App Id - TODO Images

Then create a new default data set instance with this telemetry client or define a custom dataset
to read from this client.

```c#
...
    var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions("apiKey", "appId"));
    var dataSet = new Default.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
...

```

## Custom Queries and Models
TODO
