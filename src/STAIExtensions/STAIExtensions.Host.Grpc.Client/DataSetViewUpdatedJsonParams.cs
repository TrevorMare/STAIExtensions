namespace STAIExtensions.Host.Grpc.Client;

public record class DataSetViewUpdatedJsonParams(string ViewId, string Payload)
{
    public string ViewId { get; } = ViewId;
    public string Payload { get; } = Payload;
}