using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Core;

public class DataContractQuery : Abstractions.Interfaces.IDataContractQuery
{

    #region Members
    private readonly Abstractions.Interfaces.IAIQueryApi _aiQueryApi;
    #endregion

    #region ctor
    public DataContractQuery(Abstractions.Interfaces.IAIQueryApi aiQueryApi)
    {
        this._aiQueryApi = aiQueryApi ?? throw new ArgumentNullException(nameof(aiQueryApi));
    }
    #endregion

    #region Methods

    public async Task<IEnumerable<Abstractions.DataContracts.Availability>?> GetAvailabilityAsync()
    {
        var tableResultSet = Abstractions.Common.AzureApiDataContractSource.Availability.DisplayName();
        var query = $"{tableResultSet} | where timestamp > ago(1m) | as {tableResultSet}";

        var response = await this._aiQueryApi.ExecuteQueryAsync(query);
        if (response.Success)
        {
            
        }

        return null;
    }
    

    #endregion
    
}