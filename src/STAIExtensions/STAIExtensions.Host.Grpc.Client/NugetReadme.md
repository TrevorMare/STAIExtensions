## STAIExtensions Host Grpc Client

This library contains a default .NET Protobuf Client. It also contains a managed wrapper to ease development
of handling the connections and callbacks from and to the Grpc Server.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Usage

To use the library in a .NET project, install the package from Nuget. Once installed, either the managed client
or the built in generated Protobuf client can be used as both are exposed.

The one parameter to take note of is the Owner Id parameter used throughout the lifetime of this library.
The object instantiating this class should create a unique value for the owner (or User) to be passed to the backend.
This will ensure that the callbacks when views are updated are returned to the correct instance of the Grpc client.
A suggested approach is to either generate a new Guid for the lifespan of the connection or use a user name for this value.

It should also be noted that when a user name is used for the owner and a view's parameters are set this will update the view globally.
These changes will propagate to all other retrievals like the API and SignalR GetView data.
