using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomMeasurement
    {
        CustomMeasurement? CustomMeasurements { get; set; }
    }
}