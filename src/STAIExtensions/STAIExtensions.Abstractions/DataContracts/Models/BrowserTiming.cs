using System;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class BrowserTiming : DataContract, IHasCustomMeasurement
    {

        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
                
        [DataContractField("name")]
        public string? Name { get; set; }

        [DataContractField("networkDuration")]
        public double? NetworkDuration { get; set; }

        [DataContractField("performanceBucket")]
        public string? PerformanceBucket { get; set; }
       
        [DataContractField("processingDuration")]
        public double? ProcessingDuration { get; set; }
        
        [DataContractField("receiveDuration")]
        public double? ReceiveDuration { get; set; }
        
        [DataContractField("sendDuration")]
        public double? SendDuration { get; set; }
           
        [DataContractField("totalDuration")]
        public double? TotalDuration { get; set; }

        [DataContractField("url")]
        public string? Url { get; set; }
        #endregion
        
    }
}