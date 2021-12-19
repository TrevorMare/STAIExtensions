using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_Requests : TableRowDeserializerTests_Base<Request>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Requests.json";
    protected override string TableName => "requests";
    
}