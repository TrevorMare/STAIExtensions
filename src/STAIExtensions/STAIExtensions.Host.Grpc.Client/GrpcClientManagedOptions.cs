using Grpc.Net.Client;

namespace STAIExtensions.Host.Grpc.Client;

public class GrpcClientManagedOptions
{

    #region Properties

    public bool AutoReconnect { get; set; } = true;

    public int? AutoReconnectMaxAttempts { get; set; } = null;

    public bool UseHttp2UnencryptedSupport { get; set; } = true;

    public string ChannelUrl { get; set; }

    public string OwnerId { get; set; }

    public bool UseDefaultAuthorization { get; set; } = true;

    public string AuthBearerToken { get; set; } = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666";

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

    #endregion
    
    
}