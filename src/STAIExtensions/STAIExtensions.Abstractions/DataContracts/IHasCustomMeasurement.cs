using STAIExtensions.Abstractions.Attributes;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomMeasurement
    {
        [DataContractField("customMeasurements")]
        CustomMeasurement? CustomMeasurements { get; set; }
    }
}