using Example.Host.ApiController;
using Example.Host.ApiController.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using STAIExtensions.Core;
using STAIExtensions.Host.Api;

var builder = WebApplication.CreateBuilder(args);


TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
configuration.InstrumentationKey = "c065e6f4-03cd-472e-94fd-c0518f8463f3";
configuration.TelemetryInitializers.Add(new TelemetryInitializer());
var telemetryClient = new TelemetryClient(configuration);

builder.Services.AddScoped((s) => telemetryClient);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.UseSTAIExtensionsApiHost(() => new ApiOptions() { UseAuthorization = false });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the dataset and dataset options
var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
var dsvOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(2));
builder.Services.UseSTAIExtensions(() => dsOptions, () => dsvOptions );

builder.Services.AddHostedService<ServiceRunner>();
// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

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