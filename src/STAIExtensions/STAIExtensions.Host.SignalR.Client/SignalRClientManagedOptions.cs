namespace STAIExtensions.Host.SignalR.Client;

public class SignalRClientManagedOptions
{

    public string Endpoint { get; private set; }

    public string OwnerId { get; private set; }
    
    public bool UseDefaultAuthorization { get; set; } = true;

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