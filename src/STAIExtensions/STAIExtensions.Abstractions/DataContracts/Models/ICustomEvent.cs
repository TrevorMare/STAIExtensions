
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface ICustomEvent : IHasDefaultFields, IHasCustomMeasurement
    {

        #region Properties
        public int? ItemCount { get; set; }
        
        public string? Name { get; set; }
        #endregion
        
    }
}