## STAIExtensions Host SignalR

This library contains the .NET SignalR hosting Service. For example usage, check the Examples folder.

Please take note that this library uses a very basic Authorization method and will only check that the access token passed matches the expected token. It does not support the Owin/OAuth token validations.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.SignalR?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.SignalR/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.SignalR?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.SignalR/)

[https://www.nuget.org/packages/STAIExtensions.Host.SignalR](https://www.nuget.org/packages/STAIExtensions.Host.SignalR/)


## Usage

To use the library in a .NET project, install the package from Nuget.
Follow the examples to host the application.

```c#
  
    // Register the STAIExtensions Core Library with the options provided
    var dataSetCollectionOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions(
        null, 50);
    var viewCollectionOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(
        1000, 
        false, 
        true, 
        TimeSpan.FromMinutes(15));
    
    builder.Services.UseSTAIExtensions(() => dataSetCollectionOptions, () => viewCollectionOptions);
    
    // Now add the Grpc and the SignalR Communication services
    
    var signalROptions = new SignalRHostOptions(true, "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495");
    builder.Services.UseSTAISignalR(signalROptions);

    ...
    // The Use Routing is required by the SignalR endpoint registration
    app.UseRouting();
    
    app.UseHttpsRedirection();
    
    app.UseAuthorization();
    
    app.MapSTAISignalRHubs();

    app.MapControllers();
 ```

## Generated Js Client
A Javascript client wrapper is generated on build with a version that matches the Nuget package. 
This Js file is available on the Resources directory in Github.

[Resources Directory](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/Resources)

You will not be able to reference the file directly from Github as the content type served by Github is text/plain. 
Rather use a service like [JsDelivr](https://www.jsdelivr.com/) to proxy the script download.

### Script Reference

```html
    <script src="https://cdn.jsdelivr.net/gh/TrevorMare/STAIExtensions@main/src/STAIExtensions/Resources/STAIExtensions_v1.0.0.js" crossorigin="anonymous"></script>
```

### Script Usage
```javascript
"use strict";

const createViewName = "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
let hub = STAIExtensionsHub;
let datasetId = "";
let datasetViewId = "";

const SetupHub = function() {
    
    console.log(`Initializing hub`);
    
    hub = new STAIExtensionsHub("Trevor Mare",
        "https://localhost:5001/STAIExtensionsHub",
        "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495",
        dsUpdatedCallback,
        dsvUpdatedCallback);
    console.log(`Hub Initialised`);
}

const InitViews = function() {
    hub.GetRegisteredViews((_, views) => {
        console.log('Registered views:');
        console.log(views);
    }, (err) => {
        console.log(`An error occured listing views ${err}`)
    });
    
    // Retrieve the datasets
    hub.ListDataSets((_, response) => {
        if (response !== null && response.length) {
            datasetId = response[0].dataSetId;
            console.log(response);
            CreateView();
        }
    }, (err) => {
        console.log(`An error occured loading the datasets ${err}`);
    });
}

const CreateView = function() {
    console.log(`Creating View`);
    
    hub.CreateView(createViewName, (_, iView) => {
        console.log(`View created with Id ${iView.id}`);
        console.log(iView);
        datasetViewId = iView.id;
        AttachViewToDataSet();
    }, (err) => {
        console.log(`An error occured creating the view ${err}`);
    })
    
}

const AttachViewToDataSet = function() {
    console.log(`Attaching the view ${datasetViewId} to the dataset ${datasetId}`);
    hub.AttachViewToDataset(datasetViewId, datasetId, (_, success) => {
        console.log(`The result of attaching ${success}`);
    }, (err) => {
        console.log(`An error occured attaching the view ${err}`);
    });
}

const LoadDataSetView = function () {
    console.log(`Loading the view state`);
    hub.GetView(datasetViewId, (_, view) => {
        console.log(`Loading the view state success`);
        console.log(view);
    }, (err) => {
        console.log(`Error Loading the view state ${err}`);
    });
}

const dsUpdatedCallback = function(dsId) 
    {
        console.log(`Updated dataset with Id ${dsid}`);
    }
    
const dsvUpdatedCallback = function(dsvId) {
    console.log(`Updated dataset view with Id ${dsvId}`);
    LoadDataSetView();
}

SetupHub();

setTimeout(() => {
    // Wait for the connection to establish
    InitViews();
}, 1000)


```

## Target Frameworks

- .NET Core 3.1
- .NET 5
- .NET 6

## Dependencies

- Microsoft.AspNetCore.SignalR.Client
- STAIExtensions.Abstractions
