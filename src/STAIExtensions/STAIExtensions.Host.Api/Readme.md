## STAIExtensions Host Api

This Library contains a set of API Controllers to add to your project. It allows for the interaction with the DataSets and User Views defined in the project

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.Api?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Api/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.Api?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Api/)

[https://www.nuget.org/packages/STAIExtensions.Host.Api](https://www.nuget.org/packages/STAIExtensions.Host.Api/)


## Usage

```c#

...
builder.Services.AddControllers();
builder.Services.UseSTAIExtensionsApiHost();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
...

```

## Security Options

By default the Api Controllers will use the default Authorization setup in the library

- Authorization Header Name : x-api-key
- Key Value: 38faf88370680da3e210610e017d5a5ab626a206788cd9548466c559895e2fa0

These values can be set when registering the Api Controller. 

```c#

// Inject the Controllers and the options to expose the Api 
var controllerApiOptions = new ApiOptions()
{
    HeaderName = "x-api-key",
    UseAuthorization = true,
    AllowedApiKeys = new List<string>()
    {
        builder.Configuration["ApiAuthorizationToken"]
    }
};

builder.Services.UseSTAIExtensionsApiHost(() => controllerApiOptions);

```

## Target Frameworks

- .NET Core 3.1
- .NET 5
- .NET 6
- .NET Standard 2.1

## Dependencies

- STAIExtensions.Core
- Microsoft.AspNetCore.Mvc

