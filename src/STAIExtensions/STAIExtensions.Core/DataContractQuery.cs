using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Core;

public class DataContractQuery : Abstractions.Interfaces.IDataContractQuery
{

    #region Members
    private readonly Abstractions.ApiClient.IAIQueryApiClient _aiQueryApiClient;
    #endregion

    #region ctor
    public DataContractQuery(Abstractions.ApiClient.IAIQueryApiClient aiQueryApiClient)
    {
        this._aiQueryApiClient = aiQueryApiClient ?? throw new ArgumentNullException(nameof(aiQueryApiClient));
    }
    #endregion

    #region Methods

    
    

    #endregion
    
}