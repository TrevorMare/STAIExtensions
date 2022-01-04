using Grpc.Net.Client;

namespace STAIExtensions.Host.Grpc.Client;

public class GrpcClientManagedOptions
{
    public bool UseHttp2UnencryptedSupport { get; set; } = true;

    public string ChannelUrl { get; set; }

    public string OwnerId { get; set; }

    public GrpcChannelOptions GrpcChannelOptions { get; set; } = new();

    public GrpcClientManagedOptions(string channelUrl, string ownerId)
    {
        if (string.IsNullOrEmpty(channelUrl) || channelUrl.Trim() == "")
            throw new ArgumentNullException(nameof(channelUrl));
        if (string.IsNullOrEmpty(ownerId) || ownerId.Trim() == "")
            throw new ArgumentNullException(nameof(ownerId));
        this.ChannelUrl = channelUrl;
        this.OwnerId = ownerId;
    }
    
}