using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class PerformanceCounter : DataContract, IPerformanceCounter
    {

        #region Properties
        [DataContractField("category")]
        public string? Category { get; set; }
        
        [DataContractField("counter")]
        public string? Counter { get; set; }
        
        [DataContractField("instance")]
        public string? Instance { get; set; }
        
        [DataContractField("name")]
        public string? Name { get; set; }

        [DataContractField("value")]
        public double? Value { get; set; }
        #endregion
        
    }
}