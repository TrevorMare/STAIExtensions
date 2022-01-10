## STAIExtensions Data AzureDataExplorer

### Overview
This library is included to enable Application Insights to be read via the Azure Data Explorer Api. 
In most cases this should be sufficient for building the basic views emitted to the users. This package exposes abstract queries
that can be implemented and load custom Kusto query data from the source. By default
it is able to populate all the models found in Application Insights at this point in time.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Data.AzureDataExplorer?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Data.AzureDataExplorer/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Data.AzureDataExplorer?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Data.AzureDataExplorer/)

[https://www.nuget.org/packages/STAIExtensions.Data.AzureDataExplorer/](https://www.nuget.org/packages/STAIExtensions.Data.AzureDataExplorer/)


## Usage

To use this project to populate datasets, create a new instance of the telemetry loader
with the Api Key and App Id

Then create a new default data set instance with this telemetry client or define a custom dataset
to read from this client.

```c#
...
    var telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions("apiKey", "appId"));
    var dataSet = new Default.DataSets.DataContractDataSet(telemetryLoader, new DataContractDataSetOptions(), "MyDataSet");
...

```

## Retrieving the Api Key and App Id

Log in to the Azure portal and navigate to the Application Insights instance you want to read the data from.

Scroll down in the left menu and find the **Api Access** menu item. 

![Image 1](https://github.com/TrevorMare/STAIExtensions/blob/ac767f0c844df6364ef3bbfaeb73b4f8b59fe64b/src/STAIExtensions/Resources/azure_ai_api_access_1.png?raw=true)

The Application Id can be found at the top of the page, next we will need to set up an Api Key. Click on the Create Api Key at the top of the page.

![Image 2](https://github.com/TrevorMare/STAIExtensions/blob/main/src/STAIExtensions/Resources/azure_ai_api_access_2.png?raw=true)

At a minimum this service will require the Read Telemetry to be checked. For full integration into live telemetry, the Authenticate SDK control channel tick is also required. Next click on the Generate Key button. 

![Image 3](https://github.com/TrevorMare/STAIExtensions/blob/main/src/STAIExtensions/Resources/azure_ai_api_access_3.png?raw=true)

Copy the Generated key and use this to instantiate the Telemetry loader object. ***NOTE*** Keep this key private!

## Target Frameworks

- .NET Core 3.1
- .NET 5
- .NET 6
- .NET Standard 2.1

## Dependencies

- STAIExtensions.Core
- System.Net.Http.Json