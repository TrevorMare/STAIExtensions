namespace STAIExtensions.Abstractions.Common;

public record class MyViewInformation(string ViewId, string ViewTypeName)
{
    public string ViewId { get; } = ViewId;
    public string ViewTypeName { get; } = ViewTypeName;
}