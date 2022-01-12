﻿## Examples: Grpc Client

This is an example project connects to a hosted Grpc Server instance. It is intended to work alongside the
the Examples.Grpc.Host.Console application that creates an endpoint on https://localhost:5001.

***WARNING***: The current configuration where the datasets retrieve information from is set up as the STAIExtensions Host and
may be removed at any time.

To run this example, build the project and run it.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Usage

This example project uses *598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666* as the token to pass to the hosting service. If you change the service token, this should also be changed.

```c# 
      using var managedClient =
        new GrpcClientManaged(new GrpcClientManagedOptions(" https://localhost:5001", Guid.NewGuid().ToString())
            {
                AuthBearerToken = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666",
                UseDefaultAuthorization = true
            }
            ,logger);
```
