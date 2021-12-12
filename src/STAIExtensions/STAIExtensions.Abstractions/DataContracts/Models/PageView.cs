using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class PageView : DataContract, IHasCustomMeasurement
    {

        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement? CustomMeasurements { get; set; }

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
        
        [DataContractField("url")]
        public string? Url { get; set; }
        #endregion
        
    }
}