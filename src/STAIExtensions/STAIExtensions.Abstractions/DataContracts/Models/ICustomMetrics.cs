

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface ICustomMetrics : IHasDefaultFields
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