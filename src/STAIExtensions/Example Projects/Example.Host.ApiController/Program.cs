using Example.Host.ApiController;
using Example.Host.ApiController.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using STAIExtensions.Core;
using STAIExtensions.Host.Api;

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
builder.Services.AddSwaggerGen();

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

// Add the hosted service that will read the telemetry data
builder.Services.AddHostedService<ServiceRunner>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();