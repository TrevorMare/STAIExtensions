using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Interfaces
{
    public interface IHasCustomMeasurement
    {
        [DataContractField("customMeasurements")]
        CustomMeasurement CustomMeasurements { get; set; }
    }
}