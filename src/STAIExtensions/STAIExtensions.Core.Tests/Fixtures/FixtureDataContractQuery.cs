namespace STAIExtensions.Core.Tests.Fixtures;

public class FixtureDataContractQuery<T> : Abstractions.Queries.DataContractQuery<T> where T : Abstractions.DataContracts.Models.DataContract
{
    
    
        
    public override object BuildQueryData()
    {
        return "";
    }        
}