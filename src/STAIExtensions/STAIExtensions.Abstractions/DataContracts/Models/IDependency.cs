
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface IDependency : IHasDefaultFields, IHasCustomMeasurement
    {

        #region Properties

        public string? Data { get; set; }
        
        public double? Duration { get; set; }

        public string? Id { get; set; }

        public int? ItemCount { get; set; }
        
        public string? Name { get; set; }
        
        public string? PerformanceBucket { get; set; }
        
        public string? ResultCode { get; set; }

        public string? Success { get; set; }

        public string? Target { get; set; }

        public string? Type { get; set; }
        #endregion
    }
}