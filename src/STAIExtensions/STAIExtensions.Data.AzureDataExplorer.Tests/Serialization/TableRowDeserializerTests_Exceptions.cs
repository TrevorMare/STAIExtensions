using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_Exceptions : TableRowDeserializerTests_Base<Exception>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Exceptions.json";
    protected override string TableName => "exceptions";
    
}