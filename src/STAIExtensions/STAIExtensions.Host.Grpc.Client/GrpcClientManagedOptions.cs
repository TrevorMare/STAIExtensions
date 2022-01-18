using Grpc.Net.Client;

namespace STAIExtensions.Host.Grpc.Client;

/// <summary>
/// Options and settings for instantiating the Grpc Client
/// </summary>
public class GrpcClientManagedOptions
{

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating if the client should auto reconnect to the host 
    /// </summary>
    public bool AutoReconnect { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of reconnect attempts
    /// </summary>
    public int? AutoReconnectMaxAttempts { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating if Http 2 Unencrypted support should be added
    /// </summary>
    public bool UseHttp2UnencryptedSupport { get; set; } = true;

    /// <summary>
    /// The Url of the Grpc host channel 
    /// </summary>
    public string ChannelUrl { get; set; }

    /// <summary>
    /// The unique Owner id that will be used to communicate with the host
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the client should send the authorization token
    /// </summary>
    public bool UseDefaultAuthorization { get; set; } = true;

    /// <summary>
    /// Gets or sets the token that should be sent to the host <see cref="UseDefaultAuthorization"/>
    /// </summary>
    public string AuthBearerToken { get; set; } = "598cd5656c78fc13c4d7c274ac41f34737e6b4d0e86af5c3ab47c81674dde666";

    /// <summary>
    /// Optional Grpc Channel options that will be used to create the channel
    /// </summary>
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