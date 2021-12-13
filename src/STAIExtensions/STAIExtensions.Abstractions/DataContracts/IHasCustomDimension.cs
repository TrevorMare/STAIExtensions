using STAIExtensions.Abstractions.Attributes;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomDimension
    {
        [DataContractField("customDimensions", typeof(Serialization.CustomDimensionSeriliazation))]
        CustomDimension? CustomDimensions { get; set; }
    }
}