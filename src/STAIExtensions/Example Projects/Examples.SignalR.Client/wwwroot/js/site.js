"use strict";

const createViewName = "AvailabilityView";
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
        console.log(JSON.stringify(views));
    }, (err) => {
        console.log(`An error occured listing views ${err}`)
    });

    // Retrieve the datasets
    hub.ListDataSets((_, response) => {
        if (response !== null && response.length) {
            datasetId = response[0].dataSetId;
            console.log(JSON.stringify(response));
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
        console.log(JSON.stringify(iView));
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
        console.log(JSON.stringify(view));
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

let log = document.querySelector('#log-output');
['log','warn','error'].forEach(function (verb) {
    console[verb] = (function (method, verb, log) {
        return function (text) {
            method(text);
            var msg = document.createElement('p');
            msg.classList.add(verb);
            msg.textContent = `> ${verb}: ${text}`;
            log.appendChild(msg);
        };
    })(console[verb].bind(console), verb, log);
});

SetupHub();

setTimeout(() => {
    // Wait for the connection to establish
    InitViews();
}, 1000)

