namespace STAIExtensions.Default.DataSets.Options;


public record class LoadOptions(bool Enabled = true, int? BufferSize = 10000, int? TelemetryLoadMaxRows = 2000)
{
    public bool Enabled { get; set; } = Enabled;
    public int? BufferSize { get; set; } = BufferSize;
    public int? TelemetryLoadMaxRows { get; set; } = TelemetryLoadMaxRows;
}

public class DataContractDataSetOptions
{

    public LoadOptions Availiblity { get; set; } = new LoadOptions();

    public LoadOptions BrowserTiming { get; set; } = new LoadOptions();
    
    public LoadOptions CustomEvents { get; set; } = new LoadOptions();

    public LoadOptions CustomMetrics { get; set; } = new LoadOptions();
    
    public LoadOptions Dependencies { get; set; } = new LoadOptions();
    
    public LoadOptions Exceptions { get; set; } = new LoadOptions();

    public LoadOptions PageViews { get; set; } = new LoadOptions();
    
    public LoadOptions PerformanceCounters { get; set; } = new LoadOptions();
    
    public LoadOptions Requests { get; set; } = new LoadOptions();
    
    public LoadOptions Traces { get; set; } = new LoadOptions();
}