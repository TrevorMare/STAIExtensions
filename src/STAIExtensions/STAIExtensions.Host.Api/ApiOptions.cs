namespace STAIExtensions.Host.Api;

public class ApiOptions
{

    #region Properties

    public List<string> AllowedApiKeys { get; set; } = new()
    {
        "38faf88370680da3e210610e017d5a5ab626a206788cd9548466c559895e2fa0"
    };

    public bool UseAuthorization { get; set; } = true;

    public string HeaderName { get; set; } = "x-api-key";

    #endregion

}