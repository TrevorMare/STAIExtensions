
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface IBrowserTiming : IHasDefaultFields, IHasCustomMeasurement
    {

        #region Properties
        
        public int? ItemCount { get; set; }
                
        public string? Name { get; set; }

        public double? NetworkDuration { get; set; }

        public string? PerformanceBucket { get; set; }
       
        public double? ProcessingDuration { get; set; }
        
        public double? ReceiveDuration { get; set; }
        
        public double? SendDuration { get; set; }
           
        public double? TotalDuration { get; set; }

        public string? Url { get; set; }
        #endregion
        
    }
}