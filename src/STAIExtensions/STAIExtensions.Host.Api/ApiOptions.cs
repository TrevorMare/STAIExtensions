namespace STAIExtensions.Host.Api;

/// <summary>
/// Api Controller Host options
/// </summary>
public class ApiOptions
{

    #region Properties

    /// <summary>
    /// A list of allowed Api keys that the client can use to connect to the controllers
    /// </summary>
    public List<string> AllowedApiKeys { get; set; } = new()
    {
        "38faf88370680da3e210610e017d5a5ab626a206788cd9548466c559895e2fa0"
    };

    /// <summary>
    /// Gets or sets a value indicating the use Authorization is enabled
    /// </summary>
    public bool UseAuthorization { get; set; } = true;

    /// <summary>
    /// Gets or sets the header name of where to find the token
    /// </summary>
    public string HeaderName { get; set; } = "x-api-key";

    #endregion

}