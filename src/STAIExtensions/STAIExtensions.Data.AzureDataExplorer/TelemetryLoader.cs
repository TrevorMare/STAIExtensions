using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
[assembly:InternalsVisibleTo("STAIExtensions.Data.AzureDataExplorer.Tests")]


namespace STAIExtensions.Data.AzureDataExplorer;

public class TelemetryLoader : Abstractions.Data.ITelemetryLoader
{

    #region Members

    private readonly ILogger<TelemetryLoader>? _logger;

    private readonly TelemetryLoaderOptions _loaderOptions;

    private readonly AzureDataExplorerClient _azureDataExplorerClient; 
    #endregion

    #region ctor
    public TelemetryLoader(Func<TelemetryLoaderOptions> loaderOptionsBuilder)
        : this()
    {
        if (loaderOptionsBuilder == null)
            throw new ArgumentNullException(nameof(loaderOptionsBuilder));
        
        this._loaderOptions = loaderOptionsBuilder.Invoke();
    }
    
    public TelemetryLoader(TelemetryLoaderOptions loaderOptions)
        : this()
    {
    }

    private TelemetryLoader()
    {
        this._logger = Abstractions.DependencyExtensions.CreateLogger<TelemetryLoader>();
        
        if (this._loaderOptions == null)
            throw new ArgumentNullException(nameof(_loaderOptions));        
     
        this._azureDataExplorerClient = new AzureDataExplorerClient(this._loaderOptions);
    }
    #endregion

    #region Methods
    public void LoadTelemetry()
    {
        try
        {

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task LoadTelemetryAsync(IEnumerable<Abstractions.Queries.IDataContractQuery> queries)
    {
        try
        {

            var clientReponse = await _azureDataExplorerClient.ExecuteQueryAndGetResponseAsync("");


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
    
    
}