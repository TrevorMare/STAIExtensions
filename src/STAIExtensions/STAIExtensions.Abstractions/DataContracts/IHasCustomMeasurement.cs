using STAIExtensions.Abstractions.Attributes;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomMeasurement
    {
        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        CustomMeasurement? CustomMeasurements { get; set; }
    }
}