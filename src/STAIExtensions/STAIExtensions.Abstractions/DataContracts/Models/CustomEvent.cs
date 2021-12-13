using System;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class CustomEvent : DataContract, IHasCustomMeasurement
    {

        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("name")]
        public string? Name { get; set; }
        #endregion
        
    }
}