namespace STAIExtensions.Core;

public class DataContractService
{

    #region Members
    private readonly Abstractions.ApiClient.IAIQueryApiClient _aiQueryApiClient;
    #endregion
    
    #region ctor
    public DataContractService(Abstractions.ApiClient.IAIQueryApiClient aiQueryApiClient)
    {
        _aiQueryApiClient = aiQueryApiClient ?? throw new ArgumentNullException(nameof(aiQueryApiClient));
    }
    #endregion
    
    
    public async Task LoadDataContracts()
    {
        // Call the Api Service with the query
        string query = @"availabilityResults | top 1 by timestamp asc nulls last | as availabilityResults;
                        browserTimings | top 1 by timestamp asc nulls last | as browserTimings;
                        customEvents | top 1 by timestamp asc nulls last | as customEvents;
                        customMetrics | top 1 by timestamp asc nulls last | as customMetrics;
                        dependencies | top 1 by timestamp asc nulls last | as dependencies;
                        exceptions | top 1 by timestamp asc nulls last | as exceptions;
                        pageViews | top 1 by timestamp asc nulls last | as pageViews;
                        performanceCounters | top 1 by timestamp asc nulls last | as performanceCounters;
                        requests | top 1 by timestamp asc nulls last | as requests;
                        traces | top 1 by timestamp asc nulls last | as traces;";



    }
    
}