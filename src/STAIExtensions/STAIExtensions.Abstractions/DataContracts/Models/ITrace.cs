

namespace STAIExtensions.Abstractions.DataContracts.Models
{
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