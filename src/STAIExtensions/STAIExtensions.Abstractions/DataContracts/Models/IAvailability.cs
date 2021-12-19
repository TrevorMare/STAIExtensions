
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface IAvailability : IHasDefaultFields, IHasCustomMeasurement
    {

        #region Properties
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