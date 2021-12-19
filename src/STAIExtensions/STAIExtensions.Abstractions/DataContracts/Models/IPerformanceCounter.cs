
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface IPerformanceCounter : IHasDefaultFields
    {

        #region Properties
        public string? Category { get; set; }
        
        public string? Counter { get; set; }
        
        public string? Instance { get; set; }
        
        public string? Name { get; set; }

        public double? Value { get; set; }
        #endregion
        
    }
}