
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class Request : DataContractFull, IHasCustomMeasurement
    {

        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        public double? Duration { get; set; }

        public string? Id { get; set; }
        
        public int? ItemCount { get; set; }

        public string? Name { get; set; }
        
        public string? PerformanceBucket { get; set; }
        
        public string? ResultCode { get; set; }

        public string? Source { get; set; }

        public string? Success { get; set; }
        
        public string? Url { get; set; }
        #endregion

        
    }
}