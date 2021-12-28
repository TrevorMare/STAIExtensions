"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44309/STAIExtensionsHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("OnDataSetUpdated", function (dataSetId) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `DataSet with Id ${dataSetId} Updated`;
});

connection.on("OnDataSetViewUpdated", function (viewId) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `DataSetView with Id ${viewId} Updated`;
});

connection.on("OnDataSetViewCreated", function (iView) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `DataSetView created ${iView}`;
});


connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("createViewButton").addEventListener("click", function (event) {
 
    connection.invoke("CreateView", "STAIExtensions.Core.Views.MyErrorsView").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});