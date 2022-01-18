namespace STAIExtensions.Host.Grpc.Client;

/// <summary>
/// Model that is used in event notifications 
/// </summary>
/// <param name="ViewId"></param>
/// <param name="Payload"></param>
public record DataSetViewUpdatedJsonParams(string ViewId, string Payload)
{
    /// <summary>
    /// Gets the view Id that has been updated
    /// </summary>
    public string ViewId { get; } = ViewId;
    
    /// <summary>
    /// Contains the full Json payload of the view that was updated
    /// </summary>
    public string Payload { get; } = Payload;
}