
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    
    /// <summary>
    /// Model for Performance Counters returned from the telemetry source
    /// </summary>
    public class PerformanceCounter : DataContractFull
    {

        #region Properties
        public string? Category { get; set; }
        
        public string? Counter { get; set; }
        
        public string? Instance { get; set; }
        
        public string? Name { get; set; }

        public double? Value { get; set; }
        #endregion
        
    }
}