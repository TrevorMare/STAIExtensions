using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class PerformanceCounter : DataContract
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