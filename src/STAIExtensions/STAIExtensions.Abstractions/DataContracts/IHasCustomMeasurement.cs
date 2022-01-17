using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    
    /// <summary>
    /// Interface that adds the Custom Measurements requirement to the DataContract model
    /// </summary>
    public interface IHasCustomMeasurement
    {
        CustomMeasurement? CustomMeasurements { get; set; }
    }
}