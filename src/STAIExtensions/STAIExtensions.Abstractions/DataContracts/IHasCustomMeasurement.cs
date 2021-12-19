using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomMeasurement
    {
        ICustomMeasurement? CustomMeasurements { get; set; }
    }
}