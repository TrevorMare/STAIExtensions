namespace STAIExtensions.Core.DataSets.Options;


public record class LoadOptions(bool Enabled = true, int? BufferSize = 2000);

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