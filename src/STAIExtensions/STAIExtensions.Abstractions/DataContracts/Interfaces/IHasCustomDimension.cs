using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Interfaces
{
    public interface IHasCustomDimension
    {
        [DataContractField("customDimensions")]
        CustomDimension CustomDimensions { get; set; }
    }
}