
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    
    /// <summary>
    /// Model for Browser Timing returned from the telemetry source
    /// </summary>
    public class BrowserTiming : DataContractFull, IHasCustomMeasurement
    {

        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        public int? ItemCount { get; set; }
                
        public string? Name { get; set; }

        public double? NetworkDuration { get; set; }

        public string? PerformanceBucket { get; set; }
       
        public double? ProcessingDuration { get; set; }
        
        public double? ReceiveDuration { get; set; }
        
        public double? SendDuration { get; set; }
           
        public double? TotalDuration { get; set; }

        public string? Url { get; set; }
        #endregion

        
    }
}