using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class Trace : DataContract, ITrace
    {
        #region Properties
        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        public ICustomMeasurement? CustomMeasurements { get; set; }

        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("message")]
        public string? Message { get; set; }

        [DataContractField("severityLevel")]
        public int? SeverityLevel { get; set; }
        #endregion
    }
}