using System;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class Availability : DataContract, Interfaces.IHasCustomMeasurement
    {

        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement CustomMeasurements { get; set; }
        
        [DataContractField("duration")]
        public double? Duration { get; set; }
        
        [DataContractField("id")]
        public string Id { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("location")]
        public string Location { get; set; }
        
        [DataContractField("message")]
        public string Message { get; set; }
        
        [DataContractField("name")]
        public string Name { get; set; }

        [DataContractField("performanceBucket")]
        public string PerformanceBucket { get; set; }

        [DataContractField("size")]
        public double? Size { get; set; }

        [DataContractField("success")]
        public string Success { get; set; }
        #endregion
        
    }
}