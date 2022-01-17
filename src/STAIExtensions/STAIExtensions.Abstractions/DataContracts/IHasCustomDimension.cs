using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    /// <summary>
    /// Interface that adds the Custom Dimensions requirement to the DataContract model
    /// </summary>
    public interface IHasCustomDimension
    {
        CustomDimension? CustomDimensions { get; set; }
    }
}