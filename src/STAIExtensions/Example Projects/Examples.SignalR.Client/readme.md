![Logo](https://github.com/TrevorMare/STAIExtensions/blob/15fe0579e00cfa9763671fc33816c7251e933a7b/src/STAIExtensions/Resources/logo_full.png?raw=true)

# Examples: SignalR Client (Javascript)

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)

This project showcases the Javascript SignalR Client capabilities. It connects
to the hosting SignalR server (Be sure to start the Examples.Host.ApiFull project before running this) and
outputs a log to the UI as the events occur.

***Note***: If you changed the SignalR Authorization in the Examples.Host.ApiFull project, you
need to update the connection settings in the site.js file as well.

## Requirements

The requirements for this project at the very least is the **signalr.js** script and this
can be found on [cdnjs](https://cdnjs.com/libraries/aspnet-signalr). This project uses a local copy of this file.

To use the optionally provided managed Js client wrapper, add a reference to the script found in this repository's 
[Resources/js](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/Resources/js) directory. For each
Nuget package version of STAIExtensions.Host.SignalR a script will be made available for reference. Unfortunately the script
cannot be referenced directly and a workaround for this is to proxy the download via
[jsdelivr](https://cdn.jsdelivr.net).


```html
    <script src="~/lib/microsoft/signalr/dist/browser/signalr.js"></script>
    <!-- Optional if you do your own implementation of the SignalR hub -->
    <script src="https://cdn.jsdelivr.net/gh/TrevorMare/STAIExtensions@main/src/STAIExtensions/Resources/js/STAIExtensions_v1.0.2.js" crossorigin="anonymous"></script>
```

## site.js

The included sample site.js file uses the managed client wrapper to assist 
with the workflow to connect and create user views to be attached on the host server.

The example uses the default Authorization code shipped with the examples. Change this
accordingly if you change your hosting application.

```js
    hub = new STAIExtensionsHub("Trevor Mare",
        "https://localhost:5001/STAIExtensionsHub",
        "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495",
        dsUpdatedCallback,
        dsvUpdatedCallback);
```

***Note***: After the connection is made in the client, there is a timeout before
starting the workflow to create the views. This is to give the connection a chance to 
fully initialise before starting to make calls to the host.

```js
    // Setup the Hub Connections
    SetupHub();

    // Wait for the connection to establish
    setTimeout(() => {
        InitViews();
    }, 1000)
```

For more information on the managed js wrapper, be sure to check the documentation on
[STAIExtensions.Host.SignalR](https://github.com/TrevorMare/STAIExtensions/tree/main/src/STAIExtensions/STAIExtensions.Host.SignalR)
