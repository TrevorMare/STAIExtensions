
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class Availability : DataContractFull, IHasCustomMeasurement
    {

        #region Properties
        public CustomMeasurement? CustomMeasurements { get; set; }
        
        public double? Duration { get; set; }
        
        public string? Id { get; set; }
        
        public int? ItemCount { get; set; }
        
        public string? Location { get; set; }
        
        public string? Message { get; set; }
        
        public string? Name { get; set; }

        public string? PerformanceBucket { get; set; }

        public double? Size { get; set; }

        public string? Success { get; set; }
        #endregion

        
    }
}