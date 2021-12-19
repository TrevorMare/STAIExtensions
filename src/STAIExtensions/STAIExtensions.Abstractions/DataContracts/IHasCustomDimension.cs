using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.DataContracts
{
    public interface IHasCustomDimension
    {
        ICustomDimension? CustomDimensions { get; set; }
    }
}