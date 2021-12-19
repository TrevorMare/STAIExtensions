

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface ITrace : IHasDefaultFields, IHasCustomMeasurement
    {
        #region Properties

        public int? ItemCount { get; set; }
        
        public string? Message { get; set; }

        public int? SeverityLevel { get; set; }
        #endregion
    }
}