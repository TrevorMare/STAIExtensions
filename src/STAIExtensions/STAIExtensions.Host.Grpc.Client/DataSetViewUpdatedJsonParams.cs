namespace STAIExtensions.Host.Grpc.Client;

public record DataSetViewUpdatedJsonParams(string ViewId, string Payload)
{
    public string ViewId { get; } = ViewId;
    public string Payload { get; } = Payload;
}