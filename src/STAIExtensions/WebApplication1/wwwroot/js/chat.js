"use strict";

const createViewName = "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
let hub = STAIExtensionsHub;
let datasetId = "";
let datasetViewId = "";


const SetupHub = function() {
    
    console.log(`Initializing hub`);
    
    hub = new STAIExtensionsHub("123",
        "https://localhost:7114/STAIExtensionsHub",
        null,
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

    InitViews();
}, 1000)

