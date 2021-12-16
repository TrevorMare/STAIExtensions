namespace STAIExtensions.Abstractions.Collections;

public interface IQueryDataSet
{

    event EventHandler OnDatasetUpdated;
    
    #region Properties

    string DataSetId { get; set; }

    string AppId { get; }

    string AppKey { get; }
    
    bool Configured { get; }

    #endregion

    #region Methods
    void SetupApiConfig(string appId, string appKey);

    void AddDataQuery(Abstractions.Queries.IDataContractQuery dataContractQuery);

    void AddDataQueries(IEnumerable<Abstractions.Queries.IDataContractQuery> dataContractQueries);

    void RemoveDataQuery(Abstractions.Queries.IDataContractQuery dataContractQuery);

    void ClearDataQueries();

    void RefreshDataSet();
    
    Task RefreshDataSetAsync();

    #endregion



}