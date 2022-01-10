using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Example.Host.ApiController;

public class TelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = "Host Api Controller";
        telemetry.Context.InstrumentationKey = "c065e6f4-03cd-472e-94fd-c0518f8463f3";
    }
}