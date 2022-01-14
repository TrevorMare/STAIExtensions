![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# Examples: Host Api Full

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)

This example project can be used to test all the various client connectors provided. 
On startup it will add the Api Controllers, Grpc Host and the SignalR host with the settings
of the appsettings.json file. All the example projects are configured to work with the default
Authorization tokens and if you change this, you need to change the client applications as well.

For more information on the packages, make sure to read the documentation provided on
each of the package pages in this repository.

## Data Source
This example connects to a default hosted Application Insights instance that is fed data
from all the example projects. It might not have a lot of data present initially but if you run the examples
for a while it should generate enough for the example. To connect it to another instance, update the AppSettings.json file to point to another instance.

The DataSets are created in a background Hosted Service that runs as long as the Api process is running.

***WARNING***: The sample Application Insights instance can be removed at any time.

If you want to change the Data Source to read the telemetry data from your Application Insights instance,
check the documentation [here](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/STAIExtensions.Data.AzureDataExplorer)
to download and generate keys for access.

## Required packages

- :black_square_button: STAIExtensions.Core
- :heavy_check_mark: STAIExtensions.Data.AzureDataExplorer
- :heavy_check_mark: STAIExtensions.Default
- :heavy_check_mark: STAIExtensions.Host.Api
- :heavy_check_mark: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :heavy_check_mark: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client