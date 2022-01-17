

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    /// <summary>
    /// Model for Custom Metrics returned from the telemetry source
    /// </summary>
    public class CustomMetric : DataContractFull
    {

        #region Properties
        public string? Name { get; set; }

        public double? Value { get; set; }

        public int? ValueCount { get; set; }

        public double? ValueMax { get; set; }

        public double? ValueMin { get; set; }
        
        public double? ValueStdDev { get; set; }
        
        public double? ValueSum { get; set; }
        #endregion
        
    }
}