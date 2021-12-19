using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer;
using STAIExtensions.Data.AzureDataExplorer.Queries;

namespace STAIExtensions.Service;

public class Class1
{
    
    public void Scratch()
    {

        TelemetryLoader telemetryLoader = new STAIExtensions.Data.AzureDataExplorer.TelemetryLoader(new TelemetryLoaderOptions("abc", "cde"));

        var specifiedQuery = new AzureDataExplorerQuery<Abstractions.DataContracts.Models.IAvailability>();
        var query = AzureDataExplorerQueryFactory.BuildRequestQuery();
        var result = telemetryLoader.ExecuteQueryAsync(specifiedQuery).Result;

    }
    
}