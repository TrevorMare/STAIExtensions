using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class Trace : DataContract, Interfaces.IHasCustomMeasurement
    {
        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement CustomMeasurements { get; set; }

        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("message")]
        public string Message { get; set; }

        [DataContractField("severityLevel")]
        public int? SeverityLevel { get; set; }
        #endregion
    }
}