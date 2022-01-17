namespace STAIExtensions.Data.AzureDataExplorer;

/// <summary>
/// Azure Data Explorer Telemetry Loader Options
/// </summary>
public class TelemetryLoaderOptions : Abstractions.Data.ITelemetryLoaderOptions
{
    
    #region Properties
    /// <summary>
    /// Gets or sets the Api key to read the telemetry with
    /// </summary>
    public string ApiKey { get;  } = "";

    /// <summary>
    /// Gets or sets the App Id to read the telemetry with
    /// </summary>
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