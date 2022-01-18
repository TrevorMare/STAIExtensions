## STAIExtensions Host Api

This Library contains a set of API Controllers to add to your project. It allows for the interaction with the DataSets and User Views defined in the project

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
![Last Commit](https://img.shields.io/github/last-commit/trevormare/staiextensions?style=for-the-badge)
<a href="https://trevormare.github.io/STAIExtensions/api/STAIExtensions.Host.Api.html"><img src="https://img.shields.io/badge/Documentation-Help-informational?style=for-the-badge" /></a>

## Nuget
[![Nuget](https://img.shields.io/nuget/v/STAIExtensions.Host.Api?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Api/)
[![Nuget](https://img.shields.io/nuget/dt/STAIExtensions.Host.Api?style=for-the-badge)](https://www.nuget.org/packages/STAIExtensions.Host.Api/)

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

## XML Swagger Documentation

The XML documentation for the package is included in the Nuget package. To include it in your
output directory, you need to update you csproj to manually copy the file to the output directory.
To do this, paste the below section into your csproj project

```xml
    <!-- Include Api Controller XML  -->
    <Target Name="Add Api Controller XML Documentation" AfterTargets="ResolveReferences">
        <ItemGroup>
            <ReferenceCopyLocalPaths Include="@(ReferenceCopyLocalPaths->'%(RootDir)%(Directory)%(Filename).xml')"
                                     Condition="'%(ReferenceCopyLocalPaths.NuGetPackageId)'=='STAIExtensions.Host.Api' and Exists('%(RootDir)%(Directory)%(Filename).xml')" />
        </ItemGroup>
    </Target>
```

Next, we need to tell the swagger generation service to include the comments on the files

```c#
    builder.Services.AddSwaggerGen(options =>
    {
        // Include your own controller xml files if you have them
        // ... 
        // Include the XML Comments from the Host Api 
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "STAIExtensions.Host.Api.xml"));
    });
```

## Examples

For examples check the Examples directory. This package is used in 
- Example.Host.ApiController
- Examples.Host.ApiFull

## Target Frameworks

- :heavy_check_mark: .NET Standard 2.1
- :heavy_check_mark: .NET Core 3.1
- :heavy_check_mark: .NET 5
- :heavy_check_mark: .NET 6

## Dependencies

- :black_square_button: STAIExtensions.Abstractions
- :heavy_check_mark: STAIExtensions.Core
- :black_square_button: STAIExtensions.Data.AzureDataExplorer
- :black_square_button: STAIExtensions.Default
- :black_square_button: STAIExtensions.Host.Api
- :black_square_button: STAIExtensions.Host.Grpc
- :black_square_button: STAIExtensions.Host.Grpc.Client
- :black_square_button: STAIExtensions.Host.SignalR
- :black_square_button: STAIExtensions.Host.SignalR.Client
- :heavy_check_mark: Microsoft.AspNetCore.Mvc

