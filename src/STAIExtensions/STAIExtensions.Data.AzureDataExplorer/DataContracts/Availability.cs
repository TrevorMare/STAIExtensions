using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class Availability : DataContract, IAvailability
    {

        #region Properties
        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        public ICustomMeasurement? CustomMeasurements { get; set; }
        
        [DataContractField("duration")]
        public double? Duration { get; set; }
        
        [DataContractField("id")]
        public string? Id { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("location")]
        public string? Location { get; set; }
        
        [DataContractField("message")]
        public string? Message { get; set; }
        
        [DataContractField("name")]
        public string? Name { get; set; }

        [DataContractField("performanceBucket")]
        public string? PerformanceBucket { get; set; }

        [DataContractField("size")]
        public double? Size { get; set; }

        [DataContractField("success")]
        public string? Success { get; set; }
        #endregion
        
    }
}