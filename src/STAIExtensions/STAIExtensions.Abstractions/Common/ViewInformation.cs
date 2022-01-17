using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Common;

/// <summary>
/// Structure that is returned with the Views information
/// </summary>
/// <param name="ViewName">The name of the view</param>
/// <param name="ViewTypeName">The fully qualified type name</param>
/// <param name="DataSetViewParameterDescriptors">A definition of the allowed parameters that the view accepts</param>
public record class ViewInformation(string ViewName, string ViewTypeName, IEnumerable<DataSetViewParameterDescriptor>? DataSetViewParameterDescriptors)
{
    /// <summary>
    /// The name of the view
    /// </summary>
    public string ViewName { get; } = ViewName;
    
    /// <summary>
    /// The fully qualified type name
    /// </summary>
    public string ViewTypeName { get; } = ViewTypeName;
    
    /// <summary>
    /// A definition of the allowed parameters that the view accepts
    /// </summary>
    public IEnumerable<DataSetViewParameterDescriptor>? DataSetViewParameterDescriptors { get; } = DataSetViewParameterDescriptors;
}
