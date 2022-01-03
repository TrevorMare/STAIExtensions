namespace STAIExtensions.Data.AzureDataExplorer;

public class TelemetryLoaderOptions : Abstractions.Data.ITelemetryLoaderOptions
{
    
    #region Properties
    public string ApiKey { get;  } = "";

    public string AppId { get;  } = ""; 
    #endregion

    #region ctor

    public TelemetryLoaderOptions()
    {
        
    }
    
    public TelemetryLoaderOptions(string apiKey, string appId)
    {
        this.ApiKey = apiKey;
        this.AppId = appId;

        if (string.IsNullOrEmpty(this.ApiKey))
            throw new ArgumentNullException(nameof(apiKey));

        if (string.IsNullOrEmpty(this.AppId))
            throw new ArgumentNullException(nameof(appId));
    }
    #endregion
    
}