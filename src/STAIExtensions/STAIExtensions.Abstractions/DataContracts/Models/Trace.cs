

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    /// <summary>
    /// Model for Traces returned from the telemetry source
    /// </summary>
    public class Trace : DataContractFull, IHasCustomMeasurement
    {
        
        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }

        public int? ItemCount { get; set; }
        
        public string? Message { get; set; }

        public int? SeverityLevel { get; set; }
        #endregion
        
    }
}