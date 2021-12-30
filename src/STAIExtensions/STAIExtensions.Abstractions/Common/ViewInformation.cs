using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.Common;

public record class ViewInformation(string ViewName, string ViewTypeName, IEnumerable<DataSetViewParameterDescriptor>? DataSetViewParameterDescriptors);
