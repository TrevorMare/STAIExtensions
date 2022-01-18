using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.AIDataGenerator;

public class ApplicationInsightsDataGenerator
{
    private const string ai_InstrumentationKey = "c065e6f4-03cd-472e-94fd-c0518f8463f3";


    public void GenerateTelemetry()
    {
        int counter = 0;
        while (counter < 20)
        {
            var telemetryClient = new TelemetryClient(new TelemetryConfiguration(ai_InstrumentationKey));
            telemetryClient.Context.Cloud.RoleInstance = "localhost";
            telemetryClient.Context.Cloud.RoleName = "localhost-Generate";

            GenerateTraceData(telemetryClient);
            GenerateRequestData(telemetryClient);
            GeneratePerformanceCounterData(telemetryClient);
            GenerateMetricData(telemetryClient);
            GeneratePageViewData(telemetryClient);
            GenerateExceptionData(telemetryClient);
            GenerateDependencyData(telemetryClient);
            GenerateCustomEventData(telemetryClient);

            GenerateCustomAvailibilityData(telemetryClient);
            
            telemetryClient.Flush();
            
            counter++;
        }
    }

    private void GenerateTraceData(TelemetryClient telemetryClient)
    {
        
        telemetryClient.TrackTrace(new TraceTelemetry()
        {
            Message = "This is a custom trace message",
            Properties = { {"Property 1", "Value 1"}, {"Property 2", "Value 2"} },
            Sequence = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            SeverityLevel = SeverityLevel.Information,
            ProactiveSamplingDecision = SamplingDecision.SampledIn 
        });
    }
    
    private void GenerateRequestData(TelemetryClient telemetryClient)
    {
        using (var operation = telemetryClient.StartOperation<RequestTelemetry>("GenerateRequestData"))
        {
            operation.Telemetry.Metrics.Add("Metric 1", 100);
            operation.Telemetry.Metrics.Add("Metric 2", 100);
            
            operation.Telemetry.Properties.Add("Property 1", "Value 1");
            operation.Telemetry.Properties.Add("Property 2", "Value 2");

            operation.Telemetry.Sequence = Guid.NewGuid().ToString();
            operation.Telemetry.Source = "GenerateRequestData/post";
            operation.Telemetry.Success = true;
            operation.Telemetry.Url = new Uri("https://GenerateRequestData/post");
            operation.Telemetry.ResponseCode = "204";
            operation.Telemetry.ProactiveSamplingDecision = SamplingDecision.SampledIn;
            
            System.Threading.Thread.SpinWait(200);
        }
    }

    private void GeneratePerformanceCounterData(TelemetryClient telemetryClient)
    {
        
            var operation = new PerformanceCounterTelemetry();
            operation.Value = 200;
            operation.Timestamp = DateTime.UtcNow;
            operation.CategoryName = "PerformanceCounter";
            operation.CounterName = "SampleCPUPerformance";
            operation.InstanceName = "Localhost";
            
            operation.Properties.Add("Property 1", "Value 1");
            operation.Properties.Add("Property 2", "Value 2");

            operation.Sequence = Guid.NewGuid().ToString();
            
            telemetryClient.Track(operation);
    }
    
    private void GenerateMetricData(TelemetryClient telemetryClient)
    {
        
        var operation = new MetricTelemetry();
        operation.Value = 200;
        operation.Count = 2;
        operation.Max = 4;
        operation.Min = 0;
        operation.Name = "Metric 1";
        operation.Sum = 3;
        operation.Timestamp = DateTime.UtcNow;
        operation.MetricNamespace = "Metric Namespace";
        operation.StandardDeviation = 0.5;
        operation.Properties.Add("Property 1", "Value 1");
        operation.Properties.Add("Property 2", "Value 2");
        operation.Sequence = Guid.NewGuid().ToString();
        telemetryClient.Track(operation);
        
        telemetryClient.TrackMetric(operation);
    }

    private void GeneratePageViewData(TelemetryClient telemetryClient)
    {
        
        var operation = new PageViewTelemetry("page name");
        operation.Duration = TimeSpan.FromSeconds(2);
        operation.Id = Guid.NewGuid().ToString();
        operation.Metrics.Add("Metric 1", 100);
        operation.Metrics.Add("Metric 2", 100);
        operation.Properties.Add("Property 1", "Value 1");
        operation.Properties.Add("Property 2", "Value 2");
        operation.Sequence = Guid.NewGuid().ToString();
        operation.Timestamp = DateTime.UtcNow;
        operation.Url = new Uri("https://GenerateRequestData/post");
        
        telemetryClient.TrackPageView(operation);
    }

    private void GenerateExceptionData(TelemetryClient telemetryClient)
    {
        try
        {
            int[] x = {1, 2, 3};

            var y = x[10];

        }
        catch (System.Exception e)
        {
            var outerException = new System.Exception("This is the outer exception", e);
            
           telemetryClient.TrackException(outerException, 
               new Dictionary<string, string>() { {"Property 1", "Value 1"}, {"Property 2", "Value 2"}},
               new Dictionary<string, double>() { {"Metric1", 100}, {"Metric2", 200}}
               );
            
            
        }
        
        
    }

    private void GenerateDependencyData(TelemetryClient telemetryClient)
    {
        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("GenerateDependencyData"))
        {
            operation.Telemetry.Metrics.Add("Metric 1", 100);
            operation.Telemetry.Metrics.Add("Metric 2", 100);
            
            operation.Telemetry.Properties.Add("Property 1", "Value 1");
            operation.Telemetry.Properties.Add("Property 2", "Value 2");

            operation.Telemetry.Sequence = Guid.NewGuid().ToString();
            operation.Telemetry.Success = true;
            operation.Telemetry.ProactiveSamplingDecision = SamplingDecision.SampledIn;

            operation.Telemetry.Data = "This is my data";
            operation.Telemetry.Target = "Generate Sample Data";
            operation.Telemetry.Type = "Function";
            operation.Telemetry.ResultCode = "Success";
            
            System.Threading.Thread.SpinWait(200);
        }
        
    }
    
    private void GenerateCustomEventData(TelemetryClient telemetryClient)
    {
        var operation = new EventTelemetry("My Custom Event");

        operation.Metrics.Add("Metric 1", 100);
        operation.Metrics.Add("Metric 2", 100);
        operation.Properties.Add("Property 1", "Value 1");
        operation.Properties.Add("Property 2", "Value 2");
        operation.Sequence = Guid.NewGuid().ToString();
        operation.Timestamp = DateTime.UtcNow;
        
        telemetryClient.TrackEvent(operation);

    }
    
    private void GenerateCustomBrowserTimingsData(TelemetryClient telemetryClient)
    {
        // var operation = new Brow("My Custom Event");
        //
        // operation.Metrics.Add("Metric 1", 100);
        // operation.Metrics.Add("Metric 2", 100);
        // operation.Properties.Add("Property 1", "Value 1");
        // operation.Properties.Add("Property 2", "Value 2");
        // operation.Sequence = Guid.NewGuid().ToString();
        // operation.Timestamp = DateTime.UtcNow;
        //
        // telemetryClient.TrackEvent(operation);

    }
    
    private void GenerateCustomAvailibilityData(TelemetryClient telemetryClient)
    {

        var operation = new AvailabilityTelemetry();
        
        operation.Duration = TimeSpan.FromSeconds(5);
        operation.Id = Guid.NewGuid().ToString();
        operation.Message = "Available telemetry";
        operation.Success = true;
        operation.RunLocation = "localhost";
        operation.Metrics.Add("Metric 1", 100);
        operation.Metrics.Add("Metric 2", 100);
        operation.Properties.Add("Property 1", "Value 1");
        operation.Properties.Add("Property 2", "Value 2");
        
        telemetryClient.TrackAvailability(operation);


    }
    
}