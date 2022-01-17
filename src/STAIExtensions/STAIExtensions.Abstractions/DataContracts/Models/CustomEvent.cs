
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    
    /// <summary>
    /// Model for Custom Events returned from the telemetry source
    /// </summary>
    public class CustomEvent : DataContractFull, IHasCustomMeasurement
    {

        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        public int? ItemCount { get; set; }
        
        public string? Name { get; set; }
        #endregion

        
    }
}