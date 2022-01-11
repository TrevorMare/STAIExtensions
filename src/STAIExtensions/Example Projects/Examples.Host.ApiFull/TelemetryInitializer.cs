using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Examples.Host.ApiFull;

public class TelemetryInitializer : ITelemetryInitializer
{
    private readonly string _instrumentationKey;

    public TelemetryInitializer(IConfiguration configuration)
    {
        _instrumentationKey = configuration["InstrumentationKey"];
    }
    
    
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = "Host Api Full";
        telemetry.Context.InstrumentationKey = _instrumentationKey;
    }
}