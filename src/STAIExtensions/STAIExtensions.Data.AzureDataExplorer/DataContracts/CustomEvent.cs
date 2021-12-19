﻿using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class CustomEvent : DataContract, ICustomEvent
    {

        #region Properties
        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        public ICustomMeasurement? CustomMeasurements { get; set; }
        
        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }
        
        [DataContractField("name")]
        public string? Name { get; set; }
        #endregion
        
    }
}