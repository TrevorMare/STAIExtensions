using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Common;

public record class ViewInformation(string ViewName, string ViewTypeName, IEnumerable<DataSetViewParameterDescriptor>? DataSetViewParameterDescriptors)
{
    public string ViewName { get; } = ViewName;
    public string ViewTypeName { get; } = ViewTypeName;
    public IEnumerable<DataSetViewParameterDescriptor>? DataSetViewParameterDescriptors { get; } = DataSetViewParameterDescriptors;
}
