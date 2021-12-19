using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class Request : DataContract, IRequest
    {

        #region Properties

        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        public ICustomMeasurement? CustomMeasurements { get; set; } = null;

        [DataContractField("duration")]
        public double? Duration { get; set; }

        [DataContractField("id")]
        public string? Id { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }

        [DataContractField("name")]
        public string? Name { get; set; }
        
        [DataContractField("performanceBucket")]
        public string? PerformanceBucket { get; set; }
        
        [DataContractField("resultCode")]
        public string? ResultCode { get; set; }

        [DataContractField("source")]
        public string? Source { get; set; }

        [DataContractField("success")]
        public string? Success { get; set; }
        
        [DataContractField("url")]
        public string? Url { get; set; }
        #endregion
        
    }
}