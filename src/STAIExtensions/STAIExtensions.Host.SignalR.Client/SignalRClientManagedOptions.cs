namespace STAIExtensions.Host.SignalR.Client;

/// <summary>
/// Options for the managed SignalR client
/// </summary>
public class SignalRClientManagedOptions
{

    /// <summary>
    /// The endpoint hub to connect to
    /// </summary>
    public string Endpoint { get; private set; }

    /// <summary>
    /// The owner of the instance
    /// </summary>
    public string OwnerId { get; private set; }
    
    /// <summary>
    /// Should the authorization be sent through to the host
    /// </summary>
    public bool UseDefaultAuthorization { get; set; } = true;

    /// <summary>
    /// The token to send in the authorization 
    /// </summary>
    public string AuthBearerToken { get; set; } = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495";

    #region ctor

    public SignalRClientManagedOptions(string endpoint, string ownerId)
    {
        if (string.IsNullOrEmpty(endpoint) || endpoint.Trim() == "")
            throw new ArgumentNullException(nameof(endpoint));
        
        if (string.IsNullOrEmpty(ownerId) || ownerId.Trim() == "")
            throw new ArgumentNullException(nameof(ownerId));

        this.Endpoint = endpoint;
        this.OwnerId = ownerId;
    }
    

    #endregion
    
}