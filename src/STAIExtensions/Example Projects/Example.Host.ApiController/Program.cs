using Example.Host.ApiController;
using Example.Host.ApiController.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using STAIExtensions.Core;
using STAIExtensions.Host.Api;

var builder = WebApplication.CreateBuilder(args);

// The following line enables Application Insights telemetry collection.
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
builder.Services.AddApplicationInsightsTelemetry();

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