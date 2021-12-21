using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomDimension
    {
        CustomDimension? CustomDimensions { get; set; }
    }
}