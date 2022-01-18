using Examples.Host.ApiFull;
using Examples.Host.ApiFull.Services;
using Microsoft.ApplicationInsights.Extensibility;
using STAIExtensions.Core;
using STAIExtensions.Host.Api;
using STAIExtensions.Host.Grpc;
using STAIExtensions.Host.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Include your own controller xml files if you have them
    // ... 
    // Include the XML Comments from the Host Api 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "STAIExtensions.Host.Api.xml"));
});


// Wire up the local application insights to generate data for the DataSets
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:ConnectionString"]);
 
// Register the STAIExtensions Core Library with the options provided
var dataSetCollectionOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions(
    null, 50);
var viewCollectionOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(
    1000, 
    false, 
    true, 
    TimeSpan.FromMinutes(15));

builder.Services.UseSTAIExtensions(() => dataSetCollectionOptions, () => viewCollectionOptions);

// Now add the Grpc and the SignalR Communication services

var signalROptions = new SignalRHostOptions(true, builder.Configuration["SignalRAuthorizationToken"]);
builder.Services.UseSTAISignalR(signalROptions);

var grpcOptions = new GrpcHostOptions(true, builder.Configuration["GrpcAuthorizationToken"]);
builder.Services.UseSTAIGrpc(grpcOptions);

// Add the hosted service that will read the telemetry data
builder.Services.AddHostedService<ServiceRunner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// The Use Routing is required by the SignalR endpoint registration
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

// Use the Map the SignalR Hubs and Grpc Connections after the UseAuthorization call.
app.MapSTAISignalRHubs();
app.MapSTAIGrpc(app.Environment.IsDevelopment());

app.MapControllers();

app.Run();