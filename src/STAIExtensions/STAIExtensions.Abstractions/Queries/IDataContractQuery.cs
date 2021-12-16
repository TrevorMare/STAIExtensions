namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQuery
{

    bool Enabled { get; set; }
    
    string Alias { get; set; }

    string BuildKustoQuery();

}