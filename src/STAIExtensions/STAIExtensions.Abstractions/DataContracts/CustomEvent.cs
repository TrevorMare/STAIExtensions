using System;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class CustomEvent : DataContract, Interfaces.IHasCustomMeasurement
    {

        #region Properties
        [DataContractField("customMeasurements")]
        public CustomMeasurement CustomMeasurements { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("name")]
        public string Name { get; set; }
        #endregion
        
    }
}