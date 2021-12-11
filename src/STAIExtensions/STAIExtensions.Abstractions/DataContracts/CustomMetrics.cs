using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class CustomMetrics : DataContract
    {

        #region Properties
        [DataContractField("name")]
        public string Name { get; set; }

        [DataContractField("value")]
        public double? Value { get; set; }

        [DataContractField("valueCount")]
        public int? ValueCount { get; set; }

        [DataContractField("valueMax")]
        public double? ValueMax { get; set; }

        [DataContractField("valueMin")]
        public double? ValueMin { get; set; }
        
        [DataContractField("valueStdDev")]
        public double? ValueStdDev { get; set; }
        
        [DataContractField("valueSum")]
        public double? ValueSum { get; set; }
        #endregion
        
    }
}