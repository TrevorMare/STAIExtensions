using STAIExtensions.Core;
using STAIExtensions.Host.Api;
using STAIExtensions.Host.Services;
using STAIExtensions.Host.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(configure => configure.AddConsole())
    .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

builder.Services.AddControllers();
builder.Services.UseSTAIExtensionsApiHost();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dsOptions = new STAIExtensions.Abstractions.Collections.DataSetCollectionOptions();
var dsvOptions = new STAIExtensions.Abstractions.Collections.ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(2));

builder.Services.UseSTAIExtensions(() => dsOptions, () => dsvOptions );
builder.Services.UseSTAISignalR();
builder.Services.AddHostedService<ServiceRunner>();

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

app.MapControllers();

app.Run();