using STAIExtensions.Abstractions.Attributes;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomDimension
    {
        [DataContractField("customDimensions")]
        CustomDimension? CustomDimensions { get; set; }
    }
}