## STAIExtensions Host Grpc

This library contains the .NET Protobuf Grpc Service. It can be hosted in both console applications
and in ASPNet Web projects. For example usage, check the Examples folder.

## Nuget

```http request
https://nuget.org/TODO
```

## Usage

To use the library in a .NET project, install the package from Nuget. 
Follow the examples to host the application

## Example Code AspNet Core:

```c#
using STAIExtensions.Host.Grpc;

...

builder.Services.AddControllers();
builder.Services.UseSTAIExtensionsApiHost();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
var dsvOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(2));

builder.Services.UseSTAIExtensions(() => dsOptions, () => dsvOptions );
builder.Services.UseSTAISignalR();

// *********************
// Register the Grpc Service here
// *********************
builder.Services.UseSTAIGrpc(new GrpcHostOptions()
{
    BearerToken = "ABC",
    UseDefaultAuthorization = true
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapSTAISignalRHubs();
// *********************
// Map the Grpc Controllers here after routing and Authorization
// *********************
app.MapSTAIGrpc(app.Environment.IsDevelopment());
...

```

## Example Code Service/Console Application:

```c#

using Examples.Grpc.Host.Console.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using STAIExtensions.Core;
using STAIExtensions.Host.Grpc;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
                var dsvOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(2));
                services.UseSTAIExtensions(() => dsOptions, () => dsvOptions);
                services.UseSTAIGrpc();
                services.AddLogging(configure => configure.AddConsole());
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                services.AddHostedService<ServiceRunner>();
            }).ConfigureWebHostDefaults((webBuilder) =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.MapSTAIGrpc();
                });
                webBuilder.ConfigureKestrel(options =>
                    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http2));
            });
}

```

## Target Frameworks

- .NET Core 3.1
- .NET 5
- .NET 6

## Dependencies

- Grpc.AspNetCore
- Grpc.AspNetCore.Server.Reflection
- Grp.Tools (Internal)

