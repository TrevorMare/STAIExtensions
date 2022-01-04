## STAIExtensions Host Grpc

This library contains the .NET Protobuf Grpc Service. It currently only supports hosting via an AspNet Core application and
can be extended in future. If required, the service can be instantiated manually if the host is a service or console application.

## Nuget

```http request
https://nuget.org/TODO
```

## Usage

To use the library in a .NET project, install the package from Nuget. 


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
app.MapSTAIGrpc(app.Environment);
...

```

## Example Code Service/Console Application:



## Target Frameworks

- .NET Core 3.1
- .NET 5
- .NET 6

## Dependencies

- Grpc.AspNetCore
- Grpc.AspNetCore.Server.Reflection
- Grp.Tools (Internal)

