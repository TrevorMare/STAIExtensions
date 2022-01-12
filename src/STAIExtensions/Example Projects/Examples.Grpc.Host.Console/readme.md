## Examples: Grpc Host Console

This is an example project that hosts the Grpc Server in a console application. By default it
it spins up a Kestrel server on https://localhost:5001 endpoint.

It uses the current test Application Insights instance that hosts the samples for this project to read from and populate the default dataset.
All the views that are available in the STAIExtensions.Default project can be created in this example via the clients. 

***WARNING***: The current configuration where the datasets retrieve information from is set up as the STAIExtensions Host and
may be removed at any time.

To run this example, build the project and run it.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Usage

This example application uses the an Authorization Token with the value of *598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666*. If you change this, you will also need to change the applications connecting to it.

```c#
    ...
    
    // Create the required services for the Grpc Channels and Authorization
    services.UseSTAIGrpc(new GrpcHostOptions(BearerToken: "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666"));
    
    ...
```
