using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class Dependency : DataContract, IHasCustomMeasurement
    {

        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement? CustomMeasurements { get; set; }

        [DataContractField("data")]
        public string? Data { get; set; }
        
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

        [DataContractField("success")]
        public string? Success { get; set; }

        [DataContractField("target")]
        public string? Target { get; set; }

        [DataContractField("type")]
        public string? Type { get; set; }
        #endregion
    }
}